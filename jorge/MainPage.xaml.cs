﻿namespace jorge;

public partial class MainPage : ContentPage
{
	const int ForcaPulo = 30;
	const int MaxTempoPulando = 3;
	const int Gravidade = 5;
	const int TempoEntreFrames = 25;//ms
	const int aberturaMinima = 200;
	int Velocidade = 20;
	int tempoPulando = 0;
	bool EstaMorto = true;
	bool EstaPulando = false;
	double LarguraJanela = 0;
	double AlturaJanela = 0;
	int pontuacao = 0;

	public MainPage()
	{
		InitializeComponent();
	}
	void AplicaGravidade()
	{
		Bigas.TranslationY += Gravidade;
	}

	async Task Desenha()
	{
		while (!EstaMorto)
		{
			GerenciaCanos();
			if (EstaPulando)
				AplicaPulo();
			else
				AplicaGravidade();
			if (VericaColizao())
			{
				EstaMorto = true;
				SoundHelper.Play("morte.wav");
				FrameGameOver.IsVisible = true;
				break;
			}
			await Task.Delay(TempoEntreFrames);
		}
	}

	void OnGridClicked(object s, TappedEventArgs a)
	{
		EstaPulando = true;
	}

	void OnGameOverClicked(object s, TappedEventArgs e)
	{
		FrameGameOver.IsVisible = false;
		Inicializar();
		SoundHelper.Play("começodojogo.wav");
		Desenha();
		 SoundHelper.Play("musicadefundo.wav");
	}

	void Inicializar()
	{
		EstaMorto = false;
	Bigas.TranslationY = 0;
		Bigas.TranslationX = 0;
		CanoBaixo.TranslationX = -LarguraJanela;
		CanoCima.TranslationX = -LarguraJanela;
		pontuacao = 0;
		GerenciaCanos();
	}
	protected override void OnSizeAllocated(double w, double h)
	{
		base.OnSizeAllocated(w, h);
		LarguraJanela = w;
		AlturaJanela = h;
		if (h > 0)
    {
      CanoCima.HeightRequest  = h - Aai.HeightRequest;
      CanoBaixo.HeightRequest = h - Aai.HeightRequest;
    }
	}
	void GerenciaCanos()
	{
		CanoCima.TranslationX -= Velocidade;
		CanoBaixo.TranslationX -= Velocidade;
		if (CanoBaixo.TranslationX < -LarguraJanela)
		{
			CanoBaixo.TranslationX = 0;
			CanoCima.TranslationX = 0;
		var alturaMaxima = -(CanoBaixo.HeightRequest * 0.1);
      var alturaMinima = -(CanoBaixo.HeightRequest * 0.8);

		CanoCima.TranslationY  = Random.Shared.Next((int)alturaMinima, (int)alturaMaxima);
      CanoBaixo.TranslationY = CanoCima.HeightRequest + CanoCima.TranslationY + aberturaMinima;

      pontuacao++;
	   SoundHelper.Play("pasarcanos.wav");
      labelPontuacao.Text = "Pontuação: " + pontuacao.ToString("D5");
      if (pontuacao % 4 == 0)
        Velocidade++;
		}
	}
	bool VerificaColizaoTeto()
	{
		var minY = -AlturaJanela / 2;

		if (Bigas.TranslationY <= minY)
			return true;
		else
			return false;
	}

	bool VerificaColizaoChao()
	{
		var maxY = AlturaJanela / 2 - Aai.HeightRequest;

		if (Bigas.TranslationY >= maxY)
			return true;
		else
			return false;
	}

	bool VericaColizao()
	{
		return (!EstaMorto && (VerificaColizaoChao() || VerificaColizaoTeto() || VerificaColizaoCacto()));
	}
	bool VerificaColizaoCacto()
  {
    if (VerificaColizaoCactoBaixo() || VerificaColizaoCactoCima())
      return true;
    else
      return false;
  }


	void AplicaPulo()
	{
		Bigas.TranslationY -= ForcaPulo;
		tempoPulando++;

		if (tempoPulando >= MaxTempoPulando)
		{
			EstaPulando = false;
			tempoPulando = 0;
		}
	}
	
	
	bool VerificaColizaoCactoCima()
	{
    var posHUrubu = (LarguraJanela - 50) - (Bigas.WidthRequest / 2);
    var posVUrubu   = (AlturaJanela / 2) - (Bigas.HeightRequest / 2) + Bigas.TranslationY;

    if (
         posHUrubu >= Math.Abs(CanoCima.TranslationX) - CanoCima.WidthRequest &&
         posHUrubu <= Math.Abs(CanoCima.TranslationX) + CanoCima.WidthRequest &&
         posVUrubu   <= CanoCima.HeightRequest + CanoCima.TranslationY
       )
      return true;
    else
      return false;
  }

	bool VerificaColizaoCactoBaixo()
  {
    var posHUrubu = LarguraJanela - 50 - Bigas.WidthRequest / 2;
    var posVUrubu   = (AlturaJanela / 2) + (Bigas.HeightRequest / 2) + Bigas.TranslationY;

    var yMaxCano = CanoCima.HeightRequest + CanoCima.TranslationY + aberturaMinima;

    if (
         posHUrubu >= Math.Abs(CanoCima.TranslationX) - CanoCima.WidthRequest &&
        posHUrubu <= Math.Abs(CanoCima.TranslationX) + CanoCima.WidthRequest &&
          posVUrubu   >= yMaxCano
       )
      return true;
    else
      return false;
  }
}