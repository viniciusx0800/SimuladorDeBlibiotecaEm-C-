using System;

namespace BibliotecaConsole.Models
{
    public class Emprestimo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public int LivroId { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; } 
    }
}
