using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;

var jwtString = "{\"additionalData\":{},\"alg\":null,\"crv\":null,\"d\":null,\"dp\":null,\"dq\":null,\"e\":\"AQAB\",\"k\":null,\"keyId\":null,\"keyOps\":[],\"kid\":null,\"kty\":\"RSA\",\"n\":\"t3UCV1sgGPp1fMl_7Tx0p_HqE5-Hp0HQam-qRQh0YJWGVisvudXx9kx7vHHAgzEulxmxxyAo60_KygptUEaP3O7LJv1h9r-lwgcOQSemYWfLvU8epYkPD9q_KbfP7ByldI178C4PGCOzsd3yvAaS0ELb8hqmOs8vdFng_ZBhOaBZvRM8MMSXysN7XlPmt1gAXLlIQRmK6SuqMFwF-lkwpULEpfvu_30qRrU4Qt_6aTQnUiKjkM6YjCTgoV_Hj1m4I_qLnpRlw6hQ1AEr1Xw6oh0nmzFdLW7wwu-BMj6QRBOr7eRTIuXJUZzhpM_gErIgQao1JHulSNdwH9O1HBrTBQ\",\"oth\":null,\"p\":null,\"q\":null,\"qi\":null,\"use\":null,\"x\":null,\"x5c\":[],\"x5t\":null,\"x5tS256\":null,\"x5u\":null,\"y\":null,\"keySize\":2048,\"hasPrivateKey\":false,\"cryptoProviderFactory\":{\"cryptoProviderCache\":{},\"customCryptoProvider\":null,\"cacheSignatureProviders\":true,\"signatureProviderObjectPoolCacheSize\":32}}";


var builder = WebApplication.CreateBuilder(args);





// JWT Configuration
builder.Services.AddAuthentication("jwt")
    .AddJwtBearer("jwt", o =>
    {

        o.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
        };

        // Here we triger the message received event
        o.Events = new JwtBearerEvents()
        {
            OnMessageReceived = (ctx) =>
            {
                if (ctx.Request.Query.ContainsKey("t"))
                {
                    ctx.Token = ctx.Request.Query["t"];
                }
                return Task.CompletedTask;
            }
        };

        o.Configuration = new OpenIdConnectConfiguration()
        {
            SigningKeys =
            {
               JsonWebKey.Create(jwtString)
            }
        };

        o.MapInboundClaims = false;

    });







// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();





// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


// In this API we decoded the JWT Tokeen 
app.MapGet("/", (HttpContext ctx) =>
{
    var sub = ctx.User.FindFirst("sub")?.Value ?? "EEEEMpty";
    return sub;

});


app.MapControllers();

app.Run();
