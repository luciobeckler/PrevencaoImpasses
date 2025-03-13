using PrevencaoImpasses;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

string pasta = "C:\\Users\\lucio\\OneDrive\\Documentos\\Estudo\\IFMG\\SistemasOperacionais\\GerenciadorDeMemoria\\GerenciadorDeMemoria\\ArquivosTeste";

if (Directory.Exists(pasta))
{
    string[] files = Directory.GetFiles(pasta, "TESTE-*.txt");

    foreach (string file in files)
    {

        DirectedGraph<string> grafo = new DirectedGraph<string>();

        // Lê o conteúdo do arquivo
        string content = File.ReadAllText(file);
        List<string> dados = content.Split(Environment.NewLine).ToList();

        int numProcessos = int.Parse(dados[0]);
        dados.RemoveAt(0);

        int numRecursos = int.Parse(dados[0]);
        dados.RemoveAt(0);

        Console.WriteLine($"Processando arquivo: {file}");

        grafo = PopulaGrafo(dados);

        


    }
}

DirectedGraph<string> PopulaGrafo(List<string> dados)
{
    DirectedGraph<string> grafo = new DirectedGraph<string>();
    List<Processo> processos = GeraProcessos(dados);

    foreach (var processo in processos)
    {
        grafo.AddVertex(processo._nomeProcesso);

        foreach (var recurso in processo._recursosAlocados)
        {
            grafo.AddVertex(recurso);
            grafo.AddEdge(recurso, processo._nomeProcesso);
        }

        foreach (var recurso in processo._recursosSolicitados)
        {
            grafo.AddVertex(recurso);
            grafo.AddEdge(processo._nomeProcesso, recurso);
        }
    }

    return grafo;
}

List<Processo> GeraProcessos(List<string> dados)
{
    List<Processo> processos = new List<Processo>();

    for (int i = 0; i < dados.Count; i += 2)
    {
        List<string> recursosAlocados = new List<string>();
        List<string> recursosSolicitados = new List<string>();
        List<string> partes = Regex.Replace(dados[i], @"\s+", " ").Split(" ").ToList(); // Normaliza os tipos de espaços, necessário pois podem haver espaços inquebráveis que devem ser substituidos por espaços normais.

        //Primeira linha
        string nomeProcesso = partes[0];
        partes.RemoveAt(0);

        bool isRecurso = dados.ElementAt(0).Contains("R");
        while (isRecurso)
        {
            recursosAlocados.Add(partes.ElementAt(0));
            partes.RemoveAt(0);

            isRecurso = dados.ElementAt(0).Contains("R");
        }

        //Segunda linha
        partes.RemoveAt(0); //Remove nome do processo repetido

        isRecurso = dados.ElementAt(0).Contains("R");
        while (isRecurso)
        {
            recursosSolicitados.Add(partes.ElementAt(0));
            partes.RemoveAt(0);

            isRecurso = dados.ElementAt(0).Contains("R");
        }

        processos.Add(new Processo(nomeProcesso, recursosAlocados, recursosSolicitados));
    }

    return processos;
}