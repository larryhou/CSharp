using System;
using System.Collections.Generic;

namespace EventSystem
{
	public class CustomEventArgs : EventArgs
	{
		public CustomEventArgs(string s)
		{
			message = s;
		}

		private string message;
		public string Message
		{
			get { return message; }
			set { message = value; }
		}
	}

	class EventDispatcher
	{
		public event EventHandler<CustomEventArgs> TriggerCustomEvent;

		public void Trigger()
		{
			OnTriggerCustomEvent (new CustomEventArgs ("trigger"));
		}

		protected virtual void OnTriggerCustomEvent(CustomEventArgs e)
		{
			EventHandler<CustomEventArgs> handler = TriggerCustomEvent;
			if (handler != null) 
			{
				e.Message += String.Format (" at {0:O}", DateTime.Now);
//				e.Message += String.Format (" at {0}", DateTime.Now.ToString ("O"));
			}

			handler (this, e);
		}
	}

	class EventObserver
	{
		private string id;
		public EventObserver(string ID, EventDispatcher dispatcher)
		{
			id = ID;
			dispatcher.TriggerCustomEvent += HandleCustomEvent;
		}

		public void Call()
		{
			
		}

		void HandleCustomEvent(object sender, CustomEventArgs e)
		{
			Console.WriteLine (id + " received this message:{0}", e.Message);
		}
	}

	class MainClass
	{
		public static void Main (string[] args)
		{

			EventDispatcher dispatcher = new EventDispatcher ();

			EventObserver obs = new EventObserver ("obs#01", dispatcher);
			obs.Call ();

			dispatcher.TriggerCustomEvent += (object sender, CustomEventArgs e) => 
			{
				Console.WriteLine("closure#01 got a message:{0}", e.Message);
			};

			dispatcher.TriggerCustomEvent += delegate(object sender, CustomEventArgs e) 
			{
				Console.WriteLine("closure#02 got a message:{0}", e.Message);
			};


			dispatcher.Trigger ();

		}
	}
}
