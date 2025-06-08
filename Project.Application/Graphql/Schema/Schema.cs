using Project.Domain.Entities;
using Project.Domain.Entities.User;
using Project.Shared.Consts;
using Project.Shared.Dtos.User;
using Project.Shared.Enums;

namespace Project.Application.Graphql.Schema;

public class EntitySchema : ObjectType<Entity>
{
    protected override void Configure(IObjectTypeDescriptor<Entity> descriptor)
    {
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.CreatedAt).Type<DateTimeType>();
        descriptor.Field(f => f.CreatedBy).Type<StringType>();
        descriptor.Field(f => f.UpdatedAt).Type<DateTimeType>();
        descriptor.Field(f => f.UpdatedBy).Type<StringType>();
    }
}

public class TenantSchema : ObjectType<Tenant>
{
    protected override void Configure(IObjectTypeDescriptor<Tenant> descriptor)
    {
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.RefId).Type<NonNullType<UuidType>>();
        descriptor.Field(f => f.Companies).Type<NonNullType<ListType<NonNullType<CompanySchema>>>>();
        descriptor.Field(f => f.Employees).Type<NonNullType<ListType<NonNullType<EmployeeSchema>>>>();
        descriptor.Field(f => f.TenantBadges)
                  .Type<NonNullType<ListType<NonNullType<TenantBadgeSchema>>>>();
    }
}

public class TenantBadgeSchema : ObjectType<TenantBadge>
{
    protected override void Configure(IObjectTypeDescriptor<TenantBadge> descriptor)
    {
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.RefId).Type<NonNullType<UuidType>>();
        descriptor.Field(f => f.TenantId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.Tenant).Type<NonNullType<TenantSchema>>();
        descriptor.Field(f => f.BadgeType).Type<NonNullType<BadgeTypeSchema>>();
        descriptor.Field(f => f.Employees)
                  .Type<NonNullType<ListType<NonNullType<EmployeeSchema>>>>();
    }
}

public class CompanySchema : ObjectType<Company>
{
    protected override void Configure(IObjectTypeDescriptor<Company> descriptor)
    {
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.RefId).Type<NonNullType<UuidType>>();
        descriptor.Field(f => f.TenantId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.Tenant).Type<NonNullType<TenantSchema>>();
        descriptor.Field(f => f.Name).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.Departments).Type<NonNullType<ListType<NonNullType<DepartmentSchema>>>>();
    }
}

public class DepartmentSchema : ObjectType<Department>
{
    protected override void Configure(IObjectTypeDescriptor<Department> descriptor)
    {
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.RefId).Type<NonNullType<UuidType>>();
        descriptor.Field(f => f.TenantId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.Tenant).Type<NonNullType<TenantSchema>>();
        descriptor.Field(f => f.CompanyId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.Company).Type<NonNullType<CompanySchema>>();
        descriptor.Field(f => f.Name).Type<NonNullType<StringType>>();
    }
}

public class ProjectUserSchema : ObjectType<ProjectUser>
{
    protected override void Configure(IObjectTypeDescriptor<ProjectUser> descriptor)
    {
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.RefId).Type<NonNullType<UuidType>>();
        descriptor.Field(f => f.Email).Type<StringType>();
        descriptor.Field(f => f.Name).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.PhoneNumber).Type<StringType>();
        descriptor.Field(f => f.CreatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(f => f.UpdatedAt).Type<NonNullType<DateTimeType>>();
        descriptor.Field(f => f.Employees).Type<NonNullType<ListType<NonNullType<EmployeeSchema>>>>();
        descriptor.Field(f => f.EmployeeSelectedId).Type<IntType>();
    }
}

public class EmployeeSchema : ObjectType<Employee>
{
    protected override void Configure(IObjectTypeDescriptor<Employee> descriptor)
    {
        descriptor.Field(f => f.Id).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.RefId).Type<NonNullType<UuidType>>();
        descriptor.Field(f => f.TenantId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.Tenant).Type<NonNullType<TenantSchema>>();
        descriptor.Field(f => f.UserId).Type<NonNullType<IntType>>();
        descriptor.Field(f => f.User).Type<NonNullType<ProjectUserSchema>>();
        descriptor.Field(f => f.JobName).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.TenantBadges)
                  .Type<NonNullType<ListType<NonNullType<TenantBadgeSchema>>>>();
    }
}


public class BadgeTypeSchema : EnumType<BadgeType>
{
    protected override void Configure(IEnumTypeDescriptor<BadgeType> descriptor)
    {
        descriptor.Name("BadgeType");
        descriptor.Value(BadgeType.Analyst).Name(BadgeConsts.Analyst);
        descriptor.Value(BadgeType.Developer).Name(BadgeConsts.Developer);
        descriptor.Value(BadgeType.Manager).Name(BadgeConsts.Manager);
    }
}

public class UserProfileDtoSchema : ObjectType<UserProfileDto>
{
    protected override void Configure(IObjectTypeDescriptor<UserProfileDto> descriptor)
    {
        descriptor.Field(f => f.Id).Type<NonNullType<UuidType>>();
        descriptor.Field(f => f.EmployeeSelectedRefId).Type<UuidType>();
        descriptor.Field(f => f.Email).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.UserName).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.EmployeeDtos).Type<NonNullType<ListType<NonNullType<EmployeeDtoSchema>>>>();
    }
}

public class EmployeeDtoSchema : ObjectType<EmployeeDto>
{
    protected override void Configure(IObjectTypeDescriptor<EmployeeDto> descriptor)
    {
        descriptor.Field(f => f.Id).Type<NonNullType<UuidType>>();
        descriptor.Field(f => f.TenantId).Type<NonNullType<UuidType>>();
        descriptor.Field(f => f.Name).Type<NonNullType<StringType>>();
        descriptor.Field(f => f.BadgeDtos).Type<NonNullType<ListType<NonNullType<BadgeTypeSchema>>>>();
    }
}

