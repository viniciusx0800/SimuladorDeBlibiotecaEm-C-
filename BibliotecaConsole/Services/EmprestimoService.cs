using System.Text.Json;
using BibliotecaConsole.Models;

namespace BibliotecaConsole.Services
{
    public class EmprestimoService
    {
        private readonly string _caminhoArquivo = "Data/emprestimos.json";
        private List<Emprestimo> _emprestimos;

        public EmprestimoService()
        {
            _emprestimos = CarregarEmprestimos();
        }

        private List<Emprestimo> CarregarEmprestimos()
        {
            if (!File.Exists(_caminhoArquivo))
                return new List<Emprestimo>();

            string json = File.ReadAllText(_caminhoArquivo);
            return JsonSerializer.Deserialize<List<Emprestimo>>(json) ?? new List<Emprestimo>();
        }

        private void SalvarEmprestimos()
        {
            string json = JsonSerializer.Serialize(_emprestimos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_caminhoArquivo, json);
        }

        public void EmprestarLivro(int usuarioId, int livroId)
        {
            var novoEmprestimo = new Emprestimo
            {
                Id = _emprestimos.Count > 0 ? _emprestimos.Max(e => e.Id) + 1 : 1,
                UsuarioId = usuarioId,
                LivroId = livroId,
                DataEmprestimo = DateTime.Now,
                DataDevolucao = null
            };

            _emprestimos.Add(novoEmprestimo);
            SalvarEmprestimos();
        }

        public void DevolverLivro(int emprestimoId)
        {
            var emprestimo = _emprestimos.FirstOrDefault(e => e.Id == emprestimoId);
            if (emprestimo != null && emprestimo.DataDevolucao == null)
            {
                emprestimo.DataDevolucao = DateTime.Now;
                SalvarEmprestimos();
            }
        }

        public List<Emprestimo> ListarAtivos()
        {
            return _emprestimos.Where(e => e.DataDevolucao == null).ToList();
        }

        public List<Emprestimo> HistoricoPorUsuario(int usuarioId)
        {
            return _emprestimos.Where(e => e.UsuarioId == usuarioId).ToList();
        }

        public Emprestimo? ObterPorId(int id)
        {
            return _emprestimos.FirstOrDefault(e => e.Id == id);
        }
    }
}
