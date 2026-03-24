using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using VeterinariaWeb.Data.Infrastructure;
using VeterinariaWeb.Models;
using VeterinariaWeb.ViewModels;

namespace VeterinariaWeb.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ICategoria _categoriaDB;
        private readonly IProducto _productoDB;

        public ProductosController(ICategoria categoria, IProducto producto)
        {
            _categoriaDB = categoria;
            _productoDB = producto;
        }
        public IActionResult Index(int page = 1, string? categoria = null, string? producto = null)
        {
            var listaProductos = _productoDB.Listar();
            if (categoria != null)
                listaProductos = listaProductos.Where(p => p.CategoriaID == Convert.ToInt32(categoria)).ToList();
            if (producto != null)
                listaProductos = listaProductos.Where(p => p.Nombre.ToLower().Contains(producto.ToLower()) || 
                        p.Descripcion.ToLower().Contains(producto.ToLower())).ToList();
            var listadoCategorias = _categoriaDB.Listar();
            int registrosPorPagina = 8;
            int totalProductos = listaProductos.Count;
            int cantidadPaginas = Convert.ToInt32(Math.Ceiling((double)totalProductos / registrosPorPagina));

            int registrosOmitir = registrosPorPagina * (page -1);

            ViewBag.categorias = new SelectList(listadoCategorias, "ID", "Nombre", categoria);
            ViewBag.paginas = cantidadPaginas;
            ViewBag.paginaActual = page;
            ViewBag.categoriaActual = categoria;
            ViewBag.busquedaActual = producto;
            
            return View(listaProductos.Skip(registrosOmitir).Take(registrosPorPagina));
        }

        public IActionResult Detail(int id)
        {
            var productoBuscado = _productoDB.ObtenerPorID(id);
            return View(productoBuscado);
        }

        public IActionResult Create()
        {
            var categorias = _categoriaDB.Listar();
            ViewBag.Categorias = new SelectList(categorias, "ID", "Nombre");
            return View(new ProductoVM());
        }

        [HttpPost]
        public IActionResult Create(ProductoVM model)
        {
            if (!ModelState.IsValid) { 
                var categorias = _categoriaDB.Listar();
                ViewBag.Categorias = new SelectList(categorias, "ID", "Nombre");
                return View(model);
            }


            /*TRABAJAR CON LA IMAGEN*/
            string nombreImagen = "";

            if (model.ImageFile != null) {
                nombreImagen = $"{Guid.NewGuid().ToString()}{Path.GetExtension(model.ImageFile.FileName)}";
                var pathImagen = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/productos", nombreImagen);

                using (var stream = new FileStream(pathImagen, FileMode.Create))
                {
                    model.ImageFile.CopyTo(stream);
                }
            }

            var producto = new Producto
            {
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                CategoriaID = model.CategoriaID,
                Precio = model.Precio,
                Imagen = $"assets/img/productos/{nombreImagen}"
            };

            var exito = _productoDB.Registrar(producto);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var productoBuscado = _productoDB.ObtenerPorID(id);
            var categorias = _categoriaDB.Listar();
            ViewBag.Categorias = new SelectList(categorias, "ID", "Nombre");
            return View(productoBuscado);
        }

        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            var exito = _productoDB.Modificar(producto);
            if(exito)
                return RedirectToAction("Detail", new { id = producto.ID});
            return View(producto);
        }
    }
}
