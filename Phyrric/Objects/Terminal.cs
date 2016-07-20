using Microsoft.Xna.Framework;
using SadConsole.Consoles;
using SadConsole.Controls;

namespace Phyrric.Objects
{
	public class Terminal : Entity
	{
		public string Password { get; set; }
		public bool IsLocked { get; set; }

		public Terminal(Point location, string password)
			: base((char)240, location, Color.Crimson, Color.Transparent)
		{
			Password = password;
			IsLocked = true;
		}

		public void Unlock()
		{
			IsLocked = false;
			SetForegroundColor(Color.Green);
		}

		public override void Interact(Entity other)
		{
			System.Console.WriteLine($"Password: {Password}");
			var con = new TerminalConsole(this, Password);
			con.Show(true);
		}

		public class TerminalConsole : Window
		{
			InputBox _pwinput;
			string _password;

			public Terminal AssignedTerminal;

			public TerminalConsole(Terminal _terminal, string password)
				: base(25, 5)
			{
				AssignedTerminal = _terminal;

				_password = password;

				// Border
				var border = Util.GetPanelBorder(Width, Height);
				border.Draw(this);

				// Title
				string title = "Password Entry";
				int title_x_pos = (Width / 2) - (title.Length / 2);
				Print(title_x_pos, 0, title);

				// Controls
				// Input
				int input_width = Width - 4; // 2 spacing on each side
				int input_x_pos = (Width - input_width) / 2;
				_pwinput = new InputBox(input_width)
				{
					Position = new Point(input_x_pos, 1),
					MaxLength = input_width / 2
				};

				// ok/cancel buttons
				var ok_button = new Button(10, 1)
				{
					Text = "Ok",
					TextAlignment = System.Windows.HorizontalAlignment.Center,
					CanFocus = false
				};

				var exit_button = new Button(10, 1)
				{
					Text = "Exit",
					TextAlignment = System.Windows.HorizontalAlignment.Center,
					CanFocus = false
				};

				// Position
				ok_button.Position = new Point(Width - ok_button.Width - 2, 3);
				exit_button.Position = new Point(2, 3);

				// Theme
				var btnTheme = ok_button.Theme;
				btnTheme.Normal.Background = new Color(15, 15, 15);
				btnTheme.Normal.Foreground = Color.White;

				btnTheme.MouseOver.Background = Color.Transparent;

				ok_button.Theme = btnTheme;
				exit_button.Theme = btnTheme;

				ok_button.Theme.MouseOver.Foreground = Color.Green;
				exit_button.Theme.MouseOver.Foreground = Color.Red;

				ok_button.IsDirty = true;
				exit_button.IsDirty = true;

				// Events
				ok_button.ButtonClicked += (s, e) => tryPassword();
				exit_button.ButtonClicked += (s, e) => Hide();

				// Add controls
				Add(_pwinput);
				Add(ok_button);
				Add(exit_button);

				Center();
			}

			void tryPassword()
			{
				if (string.Equals(_password, _pwinput.Text, System.StringComparison.OrdinalIgnoreCase))
				{
					AssignedTerminal.Unlock();
					Hide();
				}
				else
				{
					PhyrricGame.GameScreen.Messages
						.PrintMessage("Wrong password!");
				}
			}
		}
	}
}
