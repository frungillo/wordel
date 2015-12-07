#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input.Touch;
using Android.Widget;

#endregion

namespace wordel
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Texture2D scenario;
		char[] vocali = { (char)65, (char)69, (char)73, (char)79, (char)85 };
		List<lettere> listaLettereInGriglia, listaLettereSelezionate;
		int xVectorScenario ;
		Vector2 vectorScenario ;


		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = true;	

		}

		private void GeneraListaLettere()
		{
			int letteraX = 261;
			int letteraY = 31;
			Random r = new Random ();
			for (int i = 0; i < 25; i++) {
				char nomeLettera;// = (char)r.Next (65, 90);
				if (i % 2 == 0) {
					nomeLettera = (char)r.Next (65, 90);
				} else {
					nomeLettera = vocali [r.Next (1, 5)];
				}
				lettere letteraDaAggiungereAllaLista = new lettere (this.Content, nomeLettera.ToString ().ToLower ());
				letteraDaAggiungereAllaLista.X = letteraX;
				letteraDaAggiungereAllaLista.Y = letteraY;
				listaLettereInGriglia.Add (letteraDaAggiungereAllaLista);
				letteraX += 117;
				if (letteraX > 729) {
					letteraY += 102;
					letteraX = 261;
				}
			}
		}

		public Texture2D rect;
		public Vector2 coor;

		protected override void Initialize ()
		{
			scenario = Content.Load<Texture2D> ("wordel\\scenario_2");


			listaLettereInGriglia= new List<lettere>();
			listaLettereSelezionate = new List<lettere> ();
			GeneraListaLettere ();
			xVectorScenario = 250;
			vectorScenario = new Vector2 (xVectorScenario, 20);


			 rect = new Texture2D(graphics.GraphicsDevice, 150, 40);

			Color[] data = new Color[150*40];
			for(int i=0; i < data.Length; ++i) data[i] = Color.Chocolate;
			rect.SetData(data);

			 coor = new Vector2(5, 200);

			base.Initialize ();
				
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			//TODO: use this.Content to load your game content here 
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			#endif

			var buttonRectangle = new Rectangle (5, 200, 150, 20);
			if (isButtonPressed ().isPressed && buttonRectangle.Contains (isButtonPressed ().x, isButtonPressed ().y)) {
				foreach (lettere item in listaLettereInGriglia) {
					//System.Threading.Thread.Sleep (7);
					item.isClicked = false;
				}
				listaLettereSelezionate.Clear ();
			}

			/*
			if (GetInput ()) {
				foreach (lettere item in listaLettereInGriglia) {
					System.Threading.Thread.Sleep (7);
					item.isClicked = false;
				}
				listaLettereSelezionate.Clear ();
			}
			*/
			foreach (lettere item in listaLettereInGriglia) {
				if (item.isClicked) {
					if (!listaLettereSelezionate.Contains (item))
						listaLettereSelezionate.Add (item);
				}
				item.Update (gameTime);
			}

			

			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.LightGray);
			SpriteFont sf = Content.Load<SpriteFont> ("Calibri_1");




			/*Inizio il disegno*/
			spriteBatch.Begin ();



			spriteBatch.Draw(rect, coor, Color.White);


			//spriteBatch.Draw (scenario, vectorScenario, Color.White);

			foreach (lettere item in listaLettereInGriglia) {
				item.draw (spriteBatch);
			}
			if (listaLettereSelezionate.Count > 0) {
				string parola="";
				foreach (lettere item in listaLettereSelezionate) {
					parola += item.NomeLettera;
				}
				spriteBatch.DrawString (sf, "Parola: " + parola, new Vector2 (500, 500), Color.Black);
			}

			spriteBatch.DrawString (sf, "Cancella", new Vector2 (5, 200), Color.Black);
			spriteBatch.End ();
            /*Fine disegno*/
			base.Draw (gameTime);
		}

		bool GetInput()
		{
			bool isUp = false;
			//Vector2 desiredVelocity = new Vector2 ();

			TouchCollection touchCollection = TouchPanel.GetState();

			if (touchCollection.Count > 0 && touchCollection[0].State == TouchLocationState.Released )
			{
				isUp = true;
			}

			return isUp;
		}

		touchPressState isButtonPressed() 
		{
			touchPressState st = new touchPressState ();
			int x = 0;
			int y = 0;
			bool isInputPressed = false;
			var touchPanelState = TouchPanel.GetState();
			var mouseState = Mouse.GetState();
			if(touchPanelState.Count >= 1)
			{
				var touch = touchPanelState[0];
				x = (int)touch.Position.X;
				y = (int)touch.Position.Y;

				isInputPressed = touch.State == TouchLocationState.Pressed || touch.State == TouchLocationState.Moved;
			}
			st.isPressed = isInputPressed;
			st.x = x;
			st.y = y;
			return st;
		}

		private struct touchPressState 
		{
			//public touchPressState(){}
			public bool isPressed{ get; set; }
			public int x { get; set; }
			public int y { get; set; }
		}

	}
}

