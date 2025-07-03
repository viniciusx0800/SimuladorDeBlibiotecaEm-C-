namespace BibliotecaConsole.Models
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public bool Disponivel { get; set; } = true;
    }
}
