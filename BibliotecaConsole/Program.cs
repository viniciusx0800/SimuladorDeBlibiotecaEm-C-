using System;
using BibliotecaConsole.Models;
using BibliotecaConsole.Services;

var usuarioService = new UsuarioService();
var livroService = new LivroService();
var emprestimoService = new EmprestimoService();

bool executando = true;

while (executando)
{
    Console.Clear();
    Console.WriteLine("==== Sistema de Biblioteca ====");
    Console.WriteLine("1. Cadastrar usuário");
    Console.WriteLine("2. Cadastrar livro");
    Console.WriteLine("3. Listar livros disponíveis");
    Console.WriteLine("4. Emprestar livro");
    Console.WriteLine("5. Devolver livro");
    Console.WriteLine("6. Listar empréstimos ativos");
    Console.WriteLine("7. Histórico de empréstimos por usuário");
    Console.WriteLine("0. Sair");
    Console.Write("Escolha uma opção: ");
    string? opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            Console.Write("Nome do usuário: ");
            string nome = Console.ReadLine()!;
            usuarioService.CadastrarUsuario(new Usuario { Nome = nome });
            Console.WriteLine("Usuário cadastrado!");
            break;

        case "2":
            Console.Write("Título do livro: ");
            string titulo = Console.ReadLine()!;
            Console.Write("Autor: ");
            string autor = Console.ReadLine()!;
            livroService.CadastrarLivro(new Livro { Titulo = titulo, Autor = autor });
            Console.WriteLine("Livro cadastrado!");
            break;

        case "3":
            var livrosDisponiveis = livroService.ListarDisponiveis();
            Console.WriteLine("\n== Livros Disponíveis ==");
            foreach (var livro in livrosDisponiveis)
            {
                Console.WriteLine($"ID: {livro.Id} | Título: {livro.Titulo} | Autor: {livro.Autor}");
            }
            break;

        case "4":
            Console.Write("ID do usuário: ");
            int idUsuarioEmp = int.Parse(Console.ReadLine()!);
            Console.Write("ID do livro: ");
            int idLivroEmp = int.Parse(Console.ReadLine()!);
            var livroEmprestimo = livroService.ObterPorId(idLivroEmp);
            if (livroEmprestimo == null || !livroEmprestimo.Disponivel)
            {
                Console.WriteLine("Livro inválido ou indisponível.");
            }
            else
            {
                emprestimoService.EmprestarLivro(idUsuarioEmp, idLivroEmp);
                livroEmprestimo.Disponivel = false;
                livroService.AtualizarLivro(livroEmprestimo);
                Console.WriteLine($"Livro \"{livroEmprestimo.Titulo}\" emprestado com sucesso.");
            }
            break;


        case "5":
            Console.Write("ID do empréstimo: ");
            int idEmprestimo = int.Parse(Console.ReadLine()!);
            var emprestimo = emprestimoService.ObterPorId(idEmprestimo);
            if (emprestimo == null || emprestimo.DataDevolucao != null)
            {
                Console.WriteLine("Empréstimo inválido.");
            }
            else
            {
                emprestimoService.DevolverLivro(idEmprestimo);
                var livroDevolucao = livroService.ObterPorId(emprestimo.LivroId);
                if (livroDevolucao != null)
                {
                    livroDevolucao.Disponivel = true;
                    livroService.AtualizarLivro(livroDevolucao);
                }
                Console.WriteLine($"Livro \"{livroDevolucao?.Titulo}\" devolvido com sucesso.");
            }
            break;


        case "6":
            var ativos = emprestimoService.ListarAtivos();
            Console.WriteLine("\n== Empréstimos Ativos ==");
            foreach (var e in ativos)
            {
                Console.WriteLine($"ID: {e.Id} | Usuário ID: {e.UsuarioId} | Livro ID: {e.LivroId} | Data: {e.DataEmprestimo:dd/MM/yyyy}");
            }
            break;

        case "7":
            Console.Write("ID do usuário: ");
            int idUsuarioHist = int.Parse(Console.ReadLine()!);
            var historico = emprestimoService.HistoricoPorUsuario(idUsuarioHist);
            Console.WriteLine("\n== Histórico de Empréstimos ==");
            foreach (var h in historico)
            {
                string status = h.DataDevolucao == null ? "Ativo" : $"Devolvido em {h.DataDevolucao:dd/MM/yyyy}";
                Console.WriteLine($"ID: {h.Id} | Livro ID: {h.LivroId} | Empréstimo: {h.DataEmprestimo:dd/MM/yyyy} | {status}");
            }
            break;

        case "0":
            executando = false;
            Console.WriteLine("Encerrando...");
            break;

        default:
            Console.WriteLine("Opção inválida!");
            break;
    }

    Console.WriteLine("\nPressione Enter para continuar...");
    Console.ReadLine();
}