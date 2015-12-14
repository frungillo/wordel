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
using System.IO;
using System.Threading;
using Android.Content;
using Android.App;

#endregion

namespace wordel
{
	
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		//Texture2D scenario;
		char[] vocali = { (char)65, (char)69, (char)73, (char)79, (char)85 };
		List<lettere> listaLettereInGriglia, listaLettereSelezionate;
		int xVectorScenario ;
		//Vector2 vectorScenario ;
		int punteggio;
	    List<string> listaParoleItaliane;
		bool MostraErroreParola = false;


		Thread tError;

		Activity _mainAct;

		public Game1 (Activity par = null)
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = true;
			_mainAct = par;

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

		private void PreparaListaParole() {
			
			StreamReader r = new StreamReader (_mainAct.Assets.Open( "words.txt"));
			while (!r.EndOfStream) {
				listaParoleItaliane.Add (r.ReadLine ().Trim ().ToLower ());

			}
		}

		public Texture2D rect;
		public Vector2 coor;

		protected override void Initialize ()
		{
			//scenario = Content.Load<Texture2D> ("wordel\\scenario_2");



			listaLettereInGriglia= new List<lettere>();
			listaLettereSelezionate = new List<lettere> ();
			listaParoleItaliane = new List<string> ();
			GeneraListaLettere ();
			xVectorScenario = 250;
			punteggio = 0;
			//vectorScenario = new Vector2 (xVectorScenario, 20);
			tError = new Thread(()=>{
				MostraErroreParola = true;
				Thread.Sleep(1000);
			 	MostraErroreParola = false;
				listaLettereSelezionate.Clear ();

			});

			PreparaListaParole();		

			/*
			rect = new Texture2D(graphics.GraphicsDevice, 150, 40);

			Color[] data = new Color[150*40];
			for(int i=0; i < data.Length; ++i) data[i] = Color.Chocolate;
			rect.SetData(data);

			 coor = new Vector2(45, 200);
			*/
			base.Initialize ();
				
		}

	
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);


		}


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

			/*Pulsante Cancella*/
			var duttonRectangleCancella = new Rectangle (5, 200, 150, 40);
			if (isButtonPressed ().isPressed && duttonRectangleCancella.Contains (isButtonPressed ().x, isButtonPressed ().y)) {
				foreach (lettere item in listaLettereInGriglia) {
					if(!item.Annullata)
						item.isClicked = false;
				}
				listaLettereSelezionate.Clear ();
			}

			/*Pulsante Conferma*/
			var buttonRectangleConferma = new Rectangle (5, 280, 150, 40);
			if (isButtonPressed ().isPressed && buttonRectangleConferma.Contains (isButtonPressed ().x, isButtonPressed().y)) {
				string parola="";
				foreach (lettere item in listaLettereSelezionate) {
					parola += item.NomeLettera;
				}
				if (listaLettereSelezionate.Count > 0) {
					if (listaParoleItaliane.Contains (parola)) {
						punteggio += parola.Length;
						foreach (lettere item in listaLettereSelezionate) {
							item.Annullata = true;
					
						}
						listaLettereSelezionate.Clear ();
					} else { //se la parola non esiste ne dizionario mostro il messaggio di errore e net tError lo elimino dopo mezzo secondo.
					
						try {
							tError.Start ();
						} catch (Exception) {
							tError = new Thread (() => {
								MostraErroreParola = true;
								Thread.Sleep (1000);
								MostraErroreParola = false;
								listaLettereSelezionate.Clear ();
							});
							tError.Start ();
						}
							
						foreach (lettere item in listaLettereInGriglia) {
							if (!item.Annullata) 
								item.isClicked = false;

						}
					}
				}

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
				if (item.isClicked && !item.Annullata) {
					if (!listaLettereSelezionate.Contains (item))
						listaLettereSelezionate.Add (item);
				}
				item.Update (gameTime);
			}

			

			base.Update (gameTime);
		}


		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.LightGray);
			SpriteFont sf = Content.Load<SpriteFont> ("Calibri_1");


			/*Inizio il disegno*/
			spriteBatch.Begin();

			//spriteBatch.Draw(rect, coor, Color.White);


			//spriteBatch.Draw (scenario, vectorScenario, Color.White);

			foreach (lettere item in listaLettereInGriglia) {
				item.draw (spriteBatch);
			}

			if (listaLettereSelezionate.Count > 0) {
				string parola = "";
				foreach (lettere item in listaLettereSelezionate) {
					parola += item.NomeLettera;
				}
				spriteBatch.DrawString (sf, "Parola: " + parola, new Vector2 (500, 500), Color.Black);
			}

			spriteBatch.DrawString (sf, "Cancella", new Vector2 (5, 200), Color.Black);
			spriteBatch.DrawString (sf, "Conferma", new Vector2 (5, 280), Color.Black);
			spriteBatch.DrawString (sf, "Punteggio: "+ punteggio, new Vector2 (5, 360), Color.Black);

			if (MostraErroreParola) {
				string parola = "";
				foreach (lettere item in listaLettereSelezionate) {
					parola += item.NomeLettera;
				}
				spriteBatch.DrawString (sf, parola+" non e' una parola valida!", new Vector2 (250, 200), Color.Black);
			}

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
			//var mouseState = Mouse.GetState();
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

