namespace Project.Server.Templates;

public static class HostedTemplates
{
    public static string NotFoundPage =>

         """
               <!DOCTYPE html>
               <html lang="pt-br">
               <head>
                   <meta charset="UTF-8">
                   <meta name="viewport" content="width=device-width, initial-scale=1.0">
                   <title>Página não encontrada</title>
                   <style>
                      
                       body {
                           margin: 0;
                           padding: 0;
                           display: flex;
                           justify-content: center;
                           align-items: center;
                           height: 100vh;
                           background-color: #fff;
                           color: #1A73E8;
                       }
               
                       .container {
                           text-align: center;
                       }
               
               
                       .error-code {
                           font-size: 12rem;
                           font-weight: 600;
                           margin: 0;
                           color: transparent;
                           -webkit-text-stroke: 2px #1A73E8; /* Borda azul */
                       }
               
                       .error-message {
                           font-size: 2.4rem;
                           font-weight: 600;
                           margin: 0;
                           color: #1A73E8;
                       }
               
                       .error-description {
                           font-size: 1.6rem;
                           font-weight: 400;
                           margin: 2rem 0;
                           color: #555;
                       }
               
                       .back-button {
                           display: inline-block;
                           padding: 1rem 2rem;
                           margin-top: 2rem;
                           font-size: 1.6rem;
                           font-weight: 600;
                           color: #fff;
                           background-color: #1A73E8;
                           text-decoration: none;
                           border-radius: 0.8rem;
                       }
               
                       .back-button:hover {
                           background-color: #1558b0;
                       }
               
                       .back-button::before {
                           content: '← ';
                           font-weight: 600;
                       }
                   </style>
               </head>
               <body>
                   <div class="container">
                       <div class="error-code">404</div>
                       <div class="error-message">Página não encontrada</div>
                       <div class="error-description">Desculpe, a página que você está procurando não foi encontrada.</div>
                       <a href="javascript:history.back()" class="back-button">Voltar para a página anterior</a>
                   </div>
               </body>
               </html>
               """;

}