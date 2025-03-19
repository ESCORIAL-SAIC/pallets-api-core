using Microsoft.EntityFrameworkCore;
using PalletsApiCore;
using PalletsApiCore.Models;

var builder = WebApplication.CreateBuilder();
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ESCORIALContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("EscorialPostgreSql")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

app.MapPost("/api/login", async (LoginDto login, ESCORIALContext context) =>
{
    var user = await context.empleado
        .Join(context.ud_empleado,
            e => e.boextension_id,
            u => u.id,
            (e, u) => new { e, u })
        .Join(context.v_persona,
            eu => eu.e.enteasociado_id,
            p => p.id,
            (eu, p) => new { eu.e, eu.u, p })
        .Where(x =>
            x.u.usuario_sistema != "" &&
            x.e.activestatus == 0 &&
            x.u.usuario_sistema == login.User &&
            x.u.password == login.Password)
        .Select(x => new
        {
            x.p.id,
            x.u.usuario_sistema,
            x.p.nombre
        })
        .FirstOrDefaultAsync();
    if (user is null)
        return Results.NotFound();
    return Results.Ok(user);
})
.WithName("login")
.WithOpenApi();

app.MapGet("api/pallets", async (string? numero, ESCORIALContext context) =>
{
    if (string.IsNullOrWhiteSpace(numero))
        return Results.BadRequest("El número de pallet es requerido");
    var pallet = await context.cenker_pallets
        .Where(cenker_pallets => cenker_pallets.codigo == numero)
        .FirstOrDefaultAsync();
    if (pallet is null)
        return Results.NotFound();
    return Results.Ok(pallet);
})
.WithName("getPallets")
.WithOpenApi();

app.MapGet("api/pallets/productos", async (string? numero, ESCORIALContext context) =>
{
    if (string.IsNullOrWhiteSpace(numero))
        return Results.BadRequest("El número de pallet es requerido");
    var palletId = await context.cenker_pallets
        .Where(cenker_pallets => cenker_pallets.codigo == numero)
        .Select(cenker_pallets => cenker_pallets.id)
        .FirstOrDefaultAsync();
    var palletProducts = await context.cenker_prod_x_pallet
        .Where(cenker_prod_x_pallet => cenker_prod_x_pallet.pallet_id == palletId)
        .ToListAsync();
    var productos = new List<Product>();
    foreach (var item in palletProducts)
    {
        var producto = await context.producto
            .FirstOrDefaultAsync(producto => producto.id == item.producto_id);
        var etiqueta = await context.vp_etiquetas
            .FirstOrDefaultAsync(vp_etiquetas => vp_etiquetas.numero == int.Parse(item.serie));
        if (producto is not null)
            productos.Add(new Product
            {
                serial = int.Parse(item.serie),
                productId = producto.id,
                productCode = producto.codigo,
                description = producto.descripcion,
                type = etiqueta?.tipo,
                maxCantByPallet = 1,
                isAvailable = true
            });
    }
    return Results.Ok(productos);
})
.WithName("getProductosByPallet")
.WithOpenApi();

app.MapGet("api/productos", async (string? tipo, int? numero, ESCORIALContext context) =>
{
    if (string.IsNullOrWhiteSpace(tipo))
        return Results.BadRequest("El tipo de producto es requerido");
    if (!numero.HasValue)
        return Results.BadRequest("El número de producto es requerido");

    var etiqueta = await context.vp_etiquetas
        .FirstOrDefaultAsync(vp_etiquetas => vp_etiquetas.tipo == tipo && vp_etiquetas.numero == numero.Value);
    if (etiqueta is null)
        return Results.NotFound();

    var producto = await context.producto
        .FirstOrDefaultAsync(producto => producto.id == etiqueta.producto_id);
    if (producto is null)
        return Results.NotFound();

    var product = new Product
    {
        serial = (int)etiqueta.numero!,
        productId = producto.id,
        productCode = producto.codigo,
        description = producto.descripcion,
        type = etiqueta.tipo,
        maxCantByPallet = etiqueta.tipo.Equals("COCINA") ? 8 : 12,
        isAvailable = await Fun.IsAvailableAsync((int)etiqueta.numero, context)
    };

    return Results.Ok(product);
})
.WithName("getProductos")
.WithOpenApi();

app.MapGet("api/cocinas", async (int? numero, ESCORIALContext context) =>
{
    var query = context.etiquetas_maestro_cocinas.AsQueryable();
    if (numero.HasValue)
        query = query.Where(c => c.numero == numero.Value);
    var cocinas = await query
        .ToListAsync();
    var productos = new List<Product>();
    foreach (var cocina in cocinas)
    {
        var producto = await context.producto
            .FirstOrDefaultAsync(producto => producto.codigo == cocina.idproducto);
        if (producto is not null)
            productos.Add(new Product
            {
                serial = cocina.numero,
                productId = producto.id,
                productCode = producto.codigo,
                description = producto.descripcion,
                type = "COCINA",
                maxCantByPallet = 8,
                isAvailable = await Fun.IsAvailableAsync(cocina.numero, context)
            });
    }
    if (numero.HasValue && productos.Count < 1)
        return Results.NotFound();
    if (productos.Count == 1)
        return Results.Ok(productos.FirstOrDefault());
    return Results.Ok(productos);
})
.WithName("getCocinas")
.WithOpenApi();

app.MapGet("api/termos", async (int? numero, ESCORIALContext context) =>
{
    var query = context.etiquetas_maestro_termotanques.AsQueryable();
    if (numero.HasValue)
        query = query.Where(c => c.numero == numero.Value);
    var termos = await query
        .ToListAsync();
    var productos = new List<Product>();
    foreach (var termo in termos)
    {
        var producto = await context.producto
            .FirstOrDefaultAsync(producto => producto.codigo == termo.idproducto);
        if (producto is not null)
            productos.Add(new Product
            {
                serial = termo.numero,
                productId = producto.id,
                productCode = producto.codigo,
                description = producto.descripcion,
                type = "TERMOTANQUE",
                maxCantByPallet = 12,
                isAvailable = await Fun.IsAvailableAsync(termo.numero, context)
            });
    }
    if (numero.HasValue && productos.Count < 1)
        return Results.NotFound();
    if (productos.Count == 1)
        return Results.Ok(productos.FirstOrDefault());
    return Results.Ok(productos);
})
.WithName("getTermos")
.WithOpenApi();

app.MapPost("api/pallets/asociar-productos", async (cenker_pallets pallet, ESCORIALContext context) =>
{
    var palletId = await context.cenker_pallets
        .FirstOrDefaultAsync(p => p.codigo == pallet.codigo);
    if (palletId is null)
        return Results.NotFound();
    var productos = pallet.Products;
    var existentes = await context.cenker_prod_x_pallet
        .Where(p => p.pallet_id == palletId.id)
        .ToListAsync();
    foreach (var item in existentes)
    {
        context.cenker_prod_x_pallet.Remove(item);
    }
    foreach (var item in productos)
    {
        var pXp = new cenker_prod_x_pallet
        {
            id = Guid.NewGuid(),
            pallet_id = palletId.id,
            producto_id = item.productId,
            activo = true,
            fecha_alta = DateTime.Now.ToString(),
            fecha_modificacion = DateTime.Now.ToString(),
            serie = item.serial.ToString()
        };
        context.cenker_prod_x_pallet.Add(pXp);
    }
    try
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return Results.NoContent();
    }
    catch (Exception)
    {
        await context.Database.RollbackTransactionAsync();
        return Results.BadRequest();
    }
});



await app.RunAsync();