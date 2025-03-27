using PrevencaoImpasses;
using System.Text.RegularExpressions;

string pasta = "D:\\";

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

    while (dados.Count > 0)
    {
        List<string> recursosAlocados = new List<string>();
        List<string> recursosSolicitados = new List<string>();

        if (string.IsNullOrWhiteSpace(dados[0]))
        {
            dados.RemoveAt(0);
            continue;
        }

        List<string> partes = Regex.Replace(dados[0], @"\s+", " ").Trim().Split(" ").ToList();
        string nomeProcesso = partes[0];
        partes.RemoveAt(0);

        while (partes.Count > 0 && partes[0].Contains("R"))
        {
            recursosAlocados.Add(partes[0]);
            partes.RemoveAt(0);
        }

        dados.RemoveAt(0);

        if (dados.Count > 0 && !string.IsNullOrWhiteSpace(dados[0]))
        {
            partes = Regex.Replace(dados[0], @"\s+", " ").Trim().Split(" ").ToList();
            if (partes.Count > 1)
                partes.RemoveAt(0);

            while (partes.Count > 0 && partes[0].Contains("R"))
            {
                recursosSolicitados.Add(partes[0]);
                partes.RemoveAt(0);
            }
            dados.RemoveAt(0);
        }

        processos.Add(new Processo(nomeProcesso, recursosAlocados, recursosSolicitados));
    }

    return processos;
}