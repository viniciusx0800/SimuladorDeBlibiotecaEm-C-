using System.Text.Json;
using BibliotecaConsole.Models;

namespace BibliotecaConsole.Services
{
    public class LivroService
    {
        private readonly string _caminhoArquivo = "Data/livros.json";
        private List<Livro> _livros;

        public LivroService()
        {
            _livros = CarregarLivros();
        }

        private List<Livro> CarregarLivros()
        {
            if (!File.Exists(_caminhoArquivo))
                return new List<Livro>();

            string json = File.ReadAllText(_caminhoArquivo);
            return JsonSerializer.Deserialize<List<Livro>>(json) ?? new List<Livro>();
        }

        private void SalvarLivros()
        {
            string json = JsonSerializer.Serialize(_livros, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_caminhoArquivo, json);
        }

        public void CadastrarLivro(Livro livro)
        {
            livro.Id = _livros.Count > 0 ? _livros.Max(l => l.Id) + 1 : 1;
            _livros.Add(livro);
            SalvarLivros();
        }

        public List<Livro> ListarTodos()
        {
            return _livros;
        }

        public List<Livro> ListarDisponiveis()
        {
            return _livros.Where(l => l.Disponivel).ToList();
        }

        public Livro? ObterPorId(int id)
        {
            return _livros.FirstOrDefault(l => l.Id == id);
        }

        public void AtualizarLivro(Livro livroAtualizado)
        {
            var index = _livros.FindIndex(l => l.Id == livroAtualizado.Id);
            if (index != -1)
            {
                _livros[index] = livroAtualizado;
                SalvarLivros();
            }
        }
    }
}
