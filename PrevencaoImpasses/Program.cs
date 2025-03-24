using PrevencaoImpasses;
using System.Text.RegularExpressions;

string pasta = "C:\\Users\\lucio\\source\\repos\\PrevencaoImpasses\\PrevencaoImpasses\\ArquivosTeste";

if (Directory.Exists(pasta))
{
    string[] files = Directory.GetFiles(pasta, "TESTE-*.txt");

    foreach (string file in files)
    {

        GrafoDirecionado<string> grafo = new GrafoDirecionado<string>();

        // Lê o conteúdo do arquivo
        string content = File.ReadAllText(file);
        List<string> dados = content.Split(Environment.NewLine).ToList();

        dados.RemoveAt(0);

        Console.WriteLine($"Processando arquivo: {file}");

        grafo = PopulaGrafo(dados);

        List<string> resultado = grafo.EncontraCiclos();

        for (int i = 0; i < resultado.Count; i++)
        {
            List<string> auxiliar = resultado[i].Split(' ').ToList();

            auxiliar.Sort();

            resultado[i] = string.Join(" ", auxiliar);
        }

        string outputFileName = Path.Combine(pasta, $"TESTE{Path.GetFileNameWithoutExtension(file).Substring(5)}-RESULTADO.txt");

        using (StreamWriter sw = new StreamWriter(outputFileName))
        {
            foreach (var item in resultado)
            {
                sw.WriteLine(item);
            }
        }

    }
}

GrafoDirecionado<string> PopulaGrafo(List<string> dados)
{
    GrafoDirecionado<string> grafo = new GrafoDirecionado<string>();
    List<Processo> processos = GeraProcessos(dados);

    foreach (var processo in processos)
    {
        grafo.AdicionarVertice(processo._nomeProcesso);

        foreach (var recurso in processo._recursosAlocados)
        {
            grafo.AdicionarAresta(recurso, processo._nomeProcesso);
        }

        foreach (var recurso in processo._recursosSolicitados)
        {
            grafo.AdicionarAresta(processo._nomeProcesso, recurso);
        }
    }

    return grafo;
}

List<Processo> GeraProcessos(List<string> dados)
{
    List<Processo> processos = new List<Processo>();

    for (int i = 0; i < dados.Count; i ++)
    {
        List<string> recursosAlocados = new List<string>();
        List<string> recursosSolicitados = new List<string>();
        List<string> partes = Regex.Replace(dados[i], @"\s+", " ").Split(" ").ToList(); // Normaliza os tipos de espaços, necessário pois podem haver espaços inquebráveis que devem ser substituidos por espaços normais.

        //Primeira linha
        string nomeProcesso = partes[0];
        partes.RemoveAt(0);

        bool isRecurso = partes.ElementAt(0).Contains("R");
        while (isRecurso)
        {
            recursosAlocados.Add(partes.ElementAt(0));
            partes.RemoveAt(0);

            isRecurso = partes.Count > 0 && partes.ElementAt(0).Contains("R");
        }

        
        dados.RemoveAt(0);
        //Segunda linha
        partes = Regex.Replace(dados[i], @"\s+", " ").Split(" ").ToList();
        
        if(partes.Count > 1)
            partes.RemoveAt(0);

        isRecurso = partes.ElementAt(0).Contains("R");
        while (isRecurso)
        {

            recursosSolicitados.Add(partes.ElementAt(0));
            partes.RemoveAt(0);

            isRecurso = partes.Count > 0 && partes.ElementAt(0).Contains("R");
        }

        processos.Add(new Processo(nomeProcesso, recursosAlocados, recursosSolicitados));
    }

    return processos;
}