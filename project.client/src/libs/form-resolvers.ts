import * as Yup from "yup";

export type FormValues = Record<string, unknown>;
export type FormErrors = Record<string, string | undefined>;
export type FormListener = (state: FormState) => void;
export type ValidationSchema = Yup.ObjectSchema<Yup.AnyObject>;

export interface FormState {
  values: FormValues;
  errors: FormErrors;
  isValid: boolean;
  isSubmitting: boolean;
  touched: Record<string, boolean>;
  isLoading: boolean;
}

export interface FormConfig {
  initialValues?: FormValues;
  validationSchema?: ValidationSchema;
  validateOnChange?: boolean;
  validateOnBlur?: boolean;
}

export interface IFormManager {
  state: FormState;
  subscribe(listener: FormListener): () => void;
  initializeAsync(fetchValues: () => Promise<FormValues>): Promise<void>;
  setValue(field: string, value: unknown): Promise<void>;
  setTouched(field: string): void;
  validateField(field: string): Promise<void>;
  validateForm(): Promise<boolean>;
  submit(): Promise<void>;
  reset(): void;
}

export class FormManager implements IFormManager {
  public state: FormState;
  private listeners: FormListener[];
  private validationSchema?: ValidationSchema;
  private config: FormConfig;

  constructor(config: FormConfig = {}) {
    this.config = {
      validateOnChange: true,
      validateOnBlur: true,
      initialValues: config.initialValues || {},
      ...config,
    };
    this.state = {
      values: { ...this.config.initialValues },
      errors: {},
      isValid: false,
      isSubmitting: false,
      touched: {},
      isLoading: false,
    };
    this.listeners = [];
    this.validationSchema = config.validationSchema;
    this.validateForm();
  }

  async initializeAsync(fetchValues: () => Promise<FormValues>): Promise<void> {
    this.state.isLoading = true;
    this.emit(this.state);

    try {
      const values = await fetchValues();
      this.config.initialValues = values;
      this.state.values = { ...values };
      await this.validateForm();
    } finally {
      this.state.isLoading = false;
      this.emit(this.state);
    }
  }

  subscribe(listener: FormListener): () => void {
    this.listeners.push(listener);
    return () => {
      this.listeners = this.listeners.filter((l) => l !== listener);
    };
  }

  private emit(state: FormState): void {
    this.listeners.forEach((listener) => listener(state));
  }

  async setValue(field: string, value: unknown): Promise<void> {
    this.state.values[field] = value;
    if (this.config.validateOnChange) {
      await this.validateField(field);
    }
    this.emit(this.state);
  }

  setTouched(field: string): void {
    this.state.touched[field] = true;
    if (this.config.validateOnBlur) {
      this.validateField(field);
    }
    this.emit(this.state);
  }

  async validateField(field: string): Promise<void> {
    if (!this.validationSchema) {
      this.state.errors[field] = undefined;
      return;
    }

    try {
      await this.validationSchema.validateAt(field, this.state.values, {
        abortEarly: false,
      });
      this.state.errors[field] = undefined;
    } catch (error) {
      if (error instanceof Yup.ValidationError) {
        this.state.errors[field] = error.message;
      }
    }

    this.state.isValid = Object.values(this.state.errors).every((err) => !err);
    this.emit(this.state);
  }

  async validateForm(): Promise<boolean> {
    if (!this.validationSchema) {
      this.state.isValid = true;
      return true;
    }

    try {
      await this.validationSchema.validate(this.state.values, {
        abortEarly: false,
      });
      this.state.errors = {};
      this.state.isValid = true;
    } catch (error) {
      if (error instanceof Yup.ValidationError) {
        this.state.errors = error.inner.reduce((acc: FormErrors, err) => {
          if (err.path) acc[err.path] = err.message;
          return acc;
        }, {});
        this.state.isValid = false;
      }
    }

    this.emit(this.state);
    return this.state.isValid;
  }

  async submit(): Promise<void> {
    this.state.isSubmitting = true;
    this.emit(this.state);

    const isValid = await this.validateForm();
    if (!isValid) {
      this.state.isSubmitting = false;
      this.emit(this.state);
      return;
    }

    this.state.isSubmitting = false;
    this.emit(this.state);
  }

  reset(): void {
    this.state = {
      values: { ...this.config.initialValues },
      errors: {},
      isValid: false,
      isSubmitting: false,
      touched: {},
      isLoading: false,
    };
    this.emit(this.state);
    this.validateForm();
  }
}
