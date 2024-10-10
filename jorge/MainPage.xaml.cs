namespace jorge;

public partial class MainPage : ContentPage
{
	const int Gravidade = 1;
	const int TempoEntreFrames = 25;
	bool EstaMorto = false;

	public MainPage()
	{
		InitializeComponent();
	}
    void AplicaGravidade()
	{
		bigas.TranslationY +=Gravidade;
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
			AplicaGravidade();
			await Task.Delay(TempoEntreFrames);
		}
	}
}
