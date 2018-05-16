namespace Access2Justice.Api
{
    using Microsoft.AspNetCore.Builder;    

    public class SwaggerConfig
    {

        public static void Register(IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Host = httpReq.Host.Value;
                    //swagger.Schemes = new List<string> { "https" };
                });
            });

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Access2Justice API");
            });

        }

    }
}
