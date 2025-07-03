using System.Text.Json;
using BibliotecaConsole.Models;

namespace BibliotecaConsole.Services
{
    public class UsuarioService
    {
        private readonly string _caminhoArquivo = "Data/usuarios.json";
        private List<Usuario> _usuarios;

        public UsuarioService()
        {
            _usuarios = CarregarUsuarios();
        }

        private List<Usuario> CarregarUsuarios()
        {
            if (!File.Exists(_caminhoArquivo))
                return new List<Usuario>();

            string json = File.ReadAllText(_caminhoArquivo);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

        private void SalvarUsuarios()
        {
            string json = JsonSerializer.Serialize(_usuarios, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_caminhoArquivo, json);
        }

        public void CadastrarUsuario(Usuario usuario)
        {
            usuario.Id = _usuarios.Count > 0 ? _usuarios.Max(u => u.Id) + 1 : 1;
            _usuarios.Add(usuario);
            SalvarUsuarios();
        }

        public List<Usuario> ListarTodos()
        {
            return _usuarios;
        }

        public Usuario? ObterPorId(int id)
        {
            return _usuarios.FirstOrDefault(u => u.Id == id);
        }
    }
}
