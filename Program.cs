using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace BibliotecaJogos
{
    class Program
    {
        static List<Membro> listaMembros = new List<Membro>();
        static List<Jogo> listaJogos = new List<Jogo>();
        static List<Emprestimo> listaEmprestimos = new List<Emprestimo>();

        static void Main(string[] args)
        {
            int opcao = 0;

            do
            {
                Console.WriteLine("\nMenu Principal");
                Console.WriteLine("1. Cadastrar membro");
                Console.WriteLine("2. Cadastrar jogo");
                Console.WriteLine("3. Registrar empréstimo");
                Console.WriteLine("4. Registrar devolução");
                Console.WriteLine("5. Listar membros");
                Console.WriteLine("6. Listar jogos");
                Console.WriteLine("7. Listar empréstimos");
                Console.WriteLine("8. Sair");
                Console.Write("Escolha uma opção: ");
                opcao = int.Parse(Console.ReadLine());

                switch (opcao)
                {
                    case 1:
                        CadastrarMembro();
                        break;
                    case 2:
                        CadastrarJogo();
                        break;
                    case 3:
                        RegistrarEmprestimo();
                        break;
                    case 4:
                        RegistrarDevolucao();
                        break;
                    case 5:
                        ListarMembros();
                        break;
                    case 6:
                        ListarJogos();
                        break;
                    case 7:
                        ListarEmprestimos();
                        break;
                    case 8:
                        Console.WriteLine("Saindo...");
                        break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

            } while (opcao != 8);
        }

        static void CadastrarMembro()
        {
            Console.Write("Nome do membro: ");
            string nome = Console.ReadLine();

            Membro membro = new Membro { Nome = nome };
            listaMembros.Add(membro);

            Console.WriteLine($"Membro cadastrado: {membro.Nome}");
        }

        static void CadastrarJogo()
        {
            Console.Write("Nome do jogo: ");
            string nome = Console.ReadLine();
            Console.Write("Gênero: ");
            string genero = Console.ReadLine();
            Console.Write("Preço: ");
            string inputPreco = Console.ReadLine();

            // Substituir vírgula por ponto para padronizar
            inputPreco = inputPreco.Replace(',', '.');

            // Tentar converter para decimal com ponto como separador
            if (!decimal.TryParse(inputPreco, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out decimal preco))
            {
                Console.WriteLine("Preço inválido! Use números com '.' ou ',' como separador decimal.");
                return;
            }

            Jogo jogo = new Jogo { Nome = nome, Genero = genero, Preco = preco };
            listaJogos.Add(jogo);

            Console.WriteLine($"Jogo cadastrado: {jogo.Nome}");
        }

        static void RegistrarEmprestimo()
        {
            Console.Write("Nome do membro: ");
            string nomeMembro = Console.ReadLine();
            Console.Write("Nome do jogo: ");
            string nomeJogo = Console.ReadLine();

            Membro membro = listaMembros.FirstOrDefault(m => m.Nome.Equals(nomeMembro, StringComparison.OrdinalIgnoreCase));
            Jogo jogo = listaJogos.FirstOrDefault(j => j.Nome.Equals(nomeJogo, StringComparison.OrdinalIgnoreCase));

            if (membro == null)
            {
                Console.WriteLine("Membro não encontrado!");
                return;
            }

            if (jogo == null)
            {
                Console.WriteLine("Jogo não encontrado!");
                return;
            }

            Emprestimo emprestimo = new Emprestimo { Membro = membro, Jogo = jogo, DataEmprestimo = DateTime.Now, Devolvido = false };
            listaEmprestimos.Add(emprestimo);

            Console.WriteLine($"Empréstimo registrado: {membro.Nome} -> {jogo.Nome}");
        }

        static void RegistrarDevolucao()
        {
            Console.Write("Nome do membro: ");
            string nomeMembro = Console.ReadLine();
            Console.Write("Nome do jogo: ");
            string nomeJogo = Console.ReadLine();

            Emprestimo emprestimo = listaEmprestimos
                .FirstOrDefault(e => e.Membro.Nome.Equals(nomeMembro, StringComparison.OrdinalIgnoreCase)
                                  && e.Jogo.Nome.Equals(nomeJogo, StringComparison.OrdinalIgnoreCase)
                                  && !e.Devolvido);

            if (emprestimo == null)
            {
                Console.WriteLine("Empréstimo não encontrado!");
                return;
            }

            emprestimo.Devolvido = true;
            emprestimo.DataDevolucao = DateTime.Now;

            Console.WriteLine($"Jogo devolvido: {emprestimo.Jogo.Nome} pelo membro {emprestimo.Membro.Nome}");
        }

        static void ListarMembros()
        {
            Console.WriteLine("\nMembros cadastrados:");
            foreach (var m in listaMembros)
            {
                Console.WriteLine($"- {m.Nome}");
            }
        }

        static void ListarJogos()
        {
            Console.WriteLine("\nJogos cadastrados:");
            foreach (var j in listaJogos)
            {
                Console.WriteLine($"- {j.Nome} | {j.Genero} | R$ {j.Preco.ToString("F2", CultureInfo.InvariantCulture)}");
            }
        }

        static void ListarEmprestimos()
        {
            Console.WriteLine("\nEmpréstimos:");
            foreach (var e in listaEmprestimos)
            {
                string status = e.Devolvido ? "Devolvido" : "Emprestado";
                Console.WriteLine($"- {e.Membro.Nome} -> {e.Jogo.Nome} | {status}");
            }
        }
    }

    class Membro
    {
        public string Nome { get; set; }
    }

    class Jogo
    {
        public string Nome { get; set; }
        public string Genero { get; set; }
        public decimal Preco { get; set; }
    }

    class Emprestimo
    {
        public Membro Membro { get; set; }
        public Jogo Jogo { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public bool Devolvido { get; set; }
    }
}
