using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MVCPracticaTiendaAEN.Data;
using MVCPracticaTiendaAEN.Models;

namespace MVCPracticaTiendaAEN.Repositories
{
    #region PROCEDURES Y VISTAS
    //    alter procedure SP_add_pedido
    //(@idLibro int, @idusuario int, @cantidad int)
    //as 
    //	Insert into pedidos values
    //    ((Select max(idPedido) from pedidos) + 1,  (SELECT MAX(IDFACTURA) FROM PEDIDOS) + 1
    //	, GETDATE(), @idLibro, @idusuario, @cantidad)
    //go

    #endregion
    public class RepositoryLibros
    {
        private LibrosContext context;

        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }

        public async Task<List<Libro>> GetAllLibros()
        {
            return await this.context.Libros.ToListAsync();
        }

        

        public async Task<List<Libro>> GetLibrosGenero(int idgenero)
        {
            return await this.context.Libros.Where(x => x.IdGenero == idgenero).ToListAsync();
        }

        public async Task<Libro> FindLibro(int idlibro)
        {
            return await this.context.Libros.Where(x => x.IdLibro == idlibro).FirstOrDefaultAsync();
        }

        public async Task<List<Genero>> GetGeneros()
        {
            return await this.context.Generos.ToListAsync();
        }



        // ZONA DE USUARIO
        // validamos sus campos y si lo encuentra devuelve el objeto user
        public async Task<Usuario> Findusuario(string email, string pass)
        {
            return await this.context.Usuarios.Where(x => x.Email == email && x.Pass == pass).FirstOrDefaultAsync();
        }

        public async Task<List<Libro>> GetLibrosFromCarrito(List<int> carritoIdsLibro)
        {
            List<Libro> libros = new List<Libro>();
            foreach(int id in carritoIdsLibro)
            {
                Libro libro = await this.FindLibro(id);
                libros.Add(libro);
            }
            return libros;
        }

        public async Task<List<VistaPedido>> GetPedidosUsuario(int idusario)
        {
            return await this.context.VistaPedido.Where(x => x.IdUsuario == idusario).ToListAsync();
        }

        public async Task<Usuario> GetUsuario (int idUsuario)
        {
            return await this.context.Usuarios.Where(x => x.Idusuario == idUsuario).FirstOrDefaultAsync();
        }

        public async Task InsertPedido(int idLibro, int idUser, int cantidad) 
        {
            string sql = "SP_add_pedido @idLibro, @idusuario, @cantidad";
            SqlParameter pamidLibro = new SqlParameter("@idLibro", idLibro);
            SqlParameter pamiduser = new SqlParameter("@idusuario", idUser);
            SqlParameter pamcan = new SqlParameter("@cantidad", cantidad);
            await this.context.Database.ExecuteSqlRawAsync(sql, pamidLibro, pamiduser, pamcan);
        }
    }
}
