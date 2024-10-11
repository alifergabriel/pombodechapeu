namespace jorge;

public partial class MainPage : ContentPage
{
	double larguraJanela = 0;

	double alturaJanela = 0;

	int velocidade = 20;

	const int Gravidade = 1;
	const int TempoEntreFrames = 25;
	bool EstaMorto = false;

	public MainPage()
	{
		InitializeComponent();
	}
    void AplicaGravidade()
	{
	  Bigas.TranslationY +=Gravidade;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		Desenha();
	}

	public async void Desenha()
	{
		while (!EstaMorto)
		{
			GerenciaCanos();
			AplicaGravidade();
			await Task.Delay(TempoEntreFrames);
		}
	}
	void Ui (object s, TappedEventArgs e)
	{
		FrameGameOver.IsVisible = false;
		EstaMorto = false;
		Inicializar();
		Desenha();
	}

	void Inicializar()
	{
		Bigas.TranslationY = 0;
	}

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
		larguraJanela = width;
		alturaJanela = height;
    }

    void GerenciaCanos(){
		CanoCima.TranslationX -= velocidade;
		CanoBaixo.TranslationX -= velocidade;
		if(CanoBaixo.TranslationX < -larguraJanela){
			CanoBaixo.TranslationX = 0;
			CanoCima.TranslationX = 0;
		}
	}

	void OnGameOverClicked(object s, TappedEventArgs e)
	{
		FrameGameOver.IsVisible = false;
		Inicializar();
		Desenha();
	}
}
