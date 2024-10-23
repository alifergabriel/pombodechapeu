namespace jorge;

public partial class MainPage : ContentPage
{
	const int Gravidade = 5;
	const int TempoEntreFrames = 25;
	const int forcaPulo = 30;
	const int maxTempoPulando = 3;
	const int aberturaMinima = 100;
	int velocidade = 20;
	int tempoPulando = 0;
	double larguraJanela = 0;
	double alturaJanela = 0;
	bool EstaMorto = true;
	bool estaPulando = false;
	int score = 0;

	public MainPage()
	{
		InitializeComponent();
	}
    void AplicaGravidade()
	{
	  Bigas.TranslationY +=Gravidade;
	}

	async void Desenha()
	{
		while (!EstaMorto)
		{
			GerenciarCanos();
			if (estaPulando)
				AplicaPulo();
			else
				AplicaGravidade();
			if (VericaColizao())
			{
				EstaMorto = true;
				FrameGameOver.IsVisible = true;
				break;
			}
			await Task.Delay(TempoEntreFrames);
		}
	}

	bool VerificaColizaoTeto()
	{
		var minY = -alturaJanela / 2;

		if (Bigas.TranslationY <= minY)
			return true;
		else
			return false;
	}

	bool VerificaColizaoChao()
	{
		var maxY = alturaJanela / 2 - Aai.HeightRequest - 30;

		if (Bigas.TranslationY >= maxY)
			return true;
		else
			return false;
	}

	bool VericaColizao()
	{
		if (!EstaMorto)
		{
			if (VerificaColizaoTeto() || VerificaColizaoChao() || VerificaColizaoCanoCima() || VerificaColisaoCanoBaixo())
			{
				return true;
			}
		}

		return false;
	}

	void OnGameOverClicked (object s, TappedEventArgs e)
	{
		FrameGameOver.IsVisible = false;
		Inicializar();
		Desenha();
	}

	void Inicializar()
	{
		EstaMorto = false;
		Bigas.TranslationY = 0;
		CanoBaixo.TranslationX = 0;
		CanoCima.TranslationX = 0;
		CanoCima.TranslationX =- larguraJanela;
		CanoBaixo.TranslationX =- larguraJanela;
		Bigas.TranslationX = 0;
		Bigas.TranslationY = 0;
		score = 0;
		GerenciarCanos();
	}

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
		larguraJanela = width;
		alturaJanela = height;
    }

    void GerenciarCanos()
	{
		CanoCima.TranslationX -= velocidade;
		CanoBaixo.TranslationX -= velocidade;
		if (CanoBaixo.TranslationX < -larguraJanela)
		{
			CanoBaixo.TranslationX = 20;
			CanoCima.TranslationX = 20;
		
			var alturaMaxima = -100;
			var alturaMinima = -CanoBaixo.HeightRequest;

			CanoCima.TranslationY = Random.Shared.Next((int)alturaMinima, (int)alturaMaxima);
			CanoBaixo.TranslationY = CanoCima.TranslationY + aberturaMinima + CanoBaixo.HeightRequest;

			score++;
			LabelScore.Text = "Canos:" + score.ToString("D3");
		}
	}

	void AplicaPulo()
	{
		Bigas.TranslationY -= forcaPulo;
		tempoPulando++;

		if (tempoPulando >= maxTempoPulando)
		{
			estaPulando = false;
			tempoPulando = 0;
		}
	}

	void OnGridClicked(object s, TappedEventArgs args)
	{
		estaPulando = true;
	}
	bool VerificaColizaoCanoCima()
	{
      var posHBigas = (larguraJanela/2)-(Bigas.WidthRequest/2);
	  var posVBigas = (larguraJanela/2)-(Bigas.HeightRequest/2)+Bigas.TranslationY;
	  if (posHBigas >=Math.Abs(CanoCima.TranslationX-CanoCima.WidthRequest)&&
	  posHBigas <=Math.Abs(CanoCima.TranslationX+CanoCima.WidthRequest)&&
	  posVBigas <=CanoCima.HeightRequest+CanoCima.TranslationY)
	  {
		return true;
	  }
	  else
	  {
		return false;
	  }
	}

	bool VerificaColisaoCanoBaixo()
	{
		var posicaoHPardal = (larguraJanela / 2) - (Bigas.WidthRequest / 2);
		var posicaoVPardal = (alturaJanela / 2) - (Bigas.HeightRequest / 2) + Bigas.TranslationY;

		if (posicaoHPardal >= Math.Abs(CanoBaixo.TranslationX) + CanoBaixo.WidthRequest && 
		posicaoHPardal <= Math.Abs(CanoBaixo.TranslationX) + CanoBaixo.WidthRequest && 
		posicaoVPardal <= CanoBaixo.HeightRequest + CanoBaixo.TranslationY)
			return true;
		else
			return false;
	}

}
