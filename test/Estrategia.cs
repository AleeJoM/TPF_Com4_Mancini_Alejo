
using System;
using System.Collections.Generic;
namespace DeepSpace
{

	class Estrategia
	{
		public String Consulta1(ArbolGeneral<Planeta> arbol)
		{
			//Variable para medir la distancia entre Bot-Raíz
			int distancia = 0;
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			//Encolo el arbol que paso por parametro
			cola.encolar(arbol);
			//Mientras la cola este llena
			while(!cola.esVacia())
			{
				//En ab1, desencolamos valores de arbol
				ArbolGeneral<Planeta> ab1 = cola.desencolar();
				//Tomo el nodo de ab1 y si es planeta de IA
				if(ab1.getDatoRaiz().EsPlanetaDeLaIA())
				{
					//Distancia toma el valor del nivel de ab1
					distancia = arbol.nivel(ab1);
				}
				//Encolamos los valores para reconstruir la cola
				foreach(var hijo in ab1.getHijos())
				{
					cola.encolar(hijo);
				}
			}
			//Retornamos el mensaje con la distancia
			return "Consulta 1: La distancia entre el planeta del Bot y la Raíz es: " + distancia;
		}
		
		public String Consulta2(ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			//Mensaje con los desencientes del bot
			string mensaje = "Descendientes del Bot: ";
			bool existe = false;
			cola.encolar(arbol);
			
			while(!cola.esVacia())
			{
				ArbolGeneral<Planeta> ab1 = cola.desencolar();
				if(existe)
				{
					//A mensaje le agregamos los valores de la poblacion de cada planeta
					mensaje = mensaje + ab1.getDatoRaiz().Poblacion() + " ";
				}
				if(ab1.getDatoRaiz().EsPlanetaDeLaIA())
				{
					//Si la raiz es planeta de IA, desencolamos valores
					existe = true;
					while(!cola.esVacia())
					{
						cola.desencolar();
					}
				}
				//Encolamos valores de nuevo, para no perderlos
				foreach(var elem in ab1.getHijos())
				{
					cola.encolar(elem);
				}
			}
			//Retornamos el string de consutla 2, con el mensaje anteriormente declarado
			return "Consulta 2: " + mensaje;
		}
		
		public String Consulta3(ArbolGeneral<Planeta> arbol)
		{
			Cola<ArbolGeneral<Planeta>> cola = new Cola<ArbolGeneral<Planeta>>();
			//Declaracion de variables a aumentar
			int nivel = 0;
			int poblacion = 0;
			int contador = 0;
			int promedio = 0;
			//Mensaje consulta 3
			string mensaje = "Consulta 3: ";
			
			cola.encolar(arbol);
			cola.encolar(null);
			
			//Mensaje toma los diferentes tipos de string a mostrar
			mensaje = mensaje + "Nivel: " + nivel;
			mensaje = mensaje + " - PoblacionTotal: "+ arbol.getDatoRaiz().Poblacion();
			mensaje = mensaje + " - Promedio: " + arbol.getDatoRaiz().Poblacion();
			
			while(!cola.esVacia())
			{
				//Asigno a ab1, los valores de la cola desencolada
				ArbolGeneral<Planeta> ab1 = cola.desencolar();
				if(ab1 == null)
				{
					if(!cola.esVacia())
					{
						//Aumento de nivel
						nivel++;
						mensaje = mensaje + "\n                   Nivel: " + nivel;
						mensaje = mensaje + " - PoblacionTotal: " + poblacion;
						mensaje = mensaje + " - Promedio: " + promedio;
						cola.encolar(null);
						//Reseteo de las variables de conteo
						poblacion = 0;
						promedio = 0;
						contador = 0;
					}
				}
				else
				{
					//Recorremos el ab1 con sus hijos nodos y los asignamos a la cola de nuevo
					foreach(var elem in ab1.getHijos())
					{
						contador++;
						//asignamos a poblacion la poblacion del planeta
						poblacion = poblacion + elem.getDatoRaiz().Poblacion();
						cola.encolar(elem);
					}
					if(ab1.getHijos().Count > 0)
					{
						//Retornamos el promedio de la poblacion total por cada planeta
						promedio = poblacion / contador;
					}
				}
			}
			//Muestreo string del mensaje
			return mensaje;
		}
		public Movimiento CalcularMovimiento(ArbolGeneral<Planeta> arbol)
		{
			//Instanciacion de listas con diferentes caminos y estrategias
			List<Planeta> ataque = estrategiaBot(arbol);
			List<Planeta> caminoAbot = obtenerCamino(arbol, true);
			List<Planeta> caminoAjugador = obtenerCamino(arbol, false);
			List<Planeta> botAjugador = caminoBotAcaminoJugador(caminoAbot, caminoAjugador);
			//Si el elemento de la lista en posicion [1] es un planeta del jugador
			if (botAjugador[1].EsPlanetaDelJugador())
			{
				//Actualizo lista con la estretegia bot-jugador
				botAjugador = calculoEstrategia(ataque, caminoAjugador);
				//Calculo del camino del bot al jugador
				ataque = caminoBotAcaminoJugador(caminoAbot, caminoAjugador);
				//Si la poblacion del primer elemento planeta es mayor que la del segundo planeta
				if (ataque[0].Poblacion() > ataque[1].Poblacion())
				{
					//Dirigimos el camino hacia el jugador
					botAjugador = caminoBotAcaminoJugador(caminoAbot, caminoAjugador);
				}
			}
			//Creamos y retornamos el movimiento para atacar, con dos planetas moviendose
			Movimiento ataque1 = new Movimiento(botAjugador[0], botAjugador[1]);
			return ataque1; //retorno el ataque
		}
		//Funcion de obtencion de camino ya sea para bot->jugador o jugador->bot
		private List<Planeta> obtenerCamino(ArbolGeneral<Planeta> arbol, bool bot)
		{
			//Creamos lista de planetas para guardar el camino
			List<Planeta> listaCamino = new List<Planeta>();
			//Llamado a funcion de obtener camino
			ObtenerCamino(arbol, listaCamino, bot);
			//Retorno el camino
			return listaCamino;
		}
		//Funcion de obtener camino con parametros de arbol, lista de planetas, y booleano (si es bot o no)
		private bool ObtenerCamino(ArbolGeneral<Planeta> arbol, List<Planeta> listaCamino, bool bot)
		{
			//Comenzamos con el camino en falso
			bool camino = false;
			//Agregamnos a la lista camino la raiz del arbol
			listaCamino.Add(arbol.getDatoRaiz());
			//Si la raiz del arbol es planeta y no es del jugador
			if ((bot && arbol.getDatoRaiz().EsPlanetaDeLaIA()) || (!bot && arbol.getDatoRaiz().EsPlanetaDelJugador()))
			{
				//Camino valor verdadero
				camino = true;
			}
			else
			{
				//Recorrido recursivo para buscar el hijo planeta del bot
				foreach (var elem in arbol.getHijos())
				{
					camino = ObtenerCamino(elem, listaCamino, bot);
					//Si el camino existe, retorno verdadero
					if (camino)
					{
						return true;
					}
					//Sino, eliminamos el ultimo elemento
					listaCamino.RemoveAt(listaCamino.Count - 1);
				}
			}
			//Retornamos el camino
			return camino;
		}
		//Funcion del camino del bot hacia el jugador
		private List<Planeta> caminoBotAcaminoJugador(List<Planeta> listaCaminoBot, List<Planeta> listaCaminoJugador)
		{
			//Listas de tipos planeta
			List<Planeta> PlanetasBot = new List<Planeta>();
			List<Planeta> PlanetaNeutral = new List<Planeta>();
			List<Planeta> PlanetasJugador = new List<Planeta>();
			List<Planeta> Ancestro = new List<Planeta>();
			List<Planeta> botAjugador = new List<Planeta>();
			Planeta ancestro1;
			bool existe = false;
			//Bucle para encontrar ancentros entre los caminos del bot y del jugador.
			for (int i = 0; i < listaCaminoBot.Count && i < listaCaminoJugador.Count; i++)
			{
				//Si el planeta en posicion I es igual en bot ey jugador
				//se añade a lista de ancestros
				if (listaCaminoBot[i] == listaCaminoJugador[i])
				{
					Ancestro.Add(listaCaminoBot[i]);
				}
			}
			ancestro1 = Ancestro[Ancestro.Count - 1];
			//Bucle para construir el camino del bot hacia el jugador 
			for (int i = listaCaminoBot.Count - 1; i >= 0; i--)
			{
				botAjugador.Add(listaCaminoBot[i]);
				//If de corte cuando el camino encuentra al ancestro
				if (listaCaminoBot[i] == ancestro1)
				{
					break;
				}
			}
			//Agrego el planeta al camino hacia el jugador
			foreach(var elem in listaCaminoJugador)
			{
				if(existe)
				{
					botAjugador.Add(elem);
				}
				if(elem == ancestro1)
				{
					existe = true;
				}
			}
			//Clasificacion de planetas
			foreach (var elem in botAjugador)
			{
				if (elem.EsPlanetaDeLaIA())
				{
					PlanetasBot.Add(elem);
				}
				if (elem.EsPlanetaDelJugador())
				{
					PlanetasJugador.Add(elem);
				}
				if (elem.EsPlanetaNeutral())
				{
					PlanetaNeutral.Add(elem);
				}
			}
			//Limpieza de camino
			botAjugador.Clear();
			//Comienzo del recorrido nuevamente
			botAjugador.Add(PlanetasBot[PlanetasBot.Count - 1]);
			foreach (var elem in PlanetaNeutral)
			{
				botAjugador.Add(elem);
			}
			botAjugador.Add(PlanetasJugador[0]);
			//Retorno de camino del bot a jugador
			return botAjugador;
		}
		//Estrategia de seleccion para el ataque
		private void listaBot(ArbolGeneral<Planeta> arbol, List<Planeta> ataque)
		{
			//Si raiz es planeta de IA
			if (arbol.getDatoRaiz().EsPlanetaDeLaIA())
			{
				//Agrego a lista de ataque
				ataque.Add(arbol.getDatoRaiz());
			}
			//Sino cumple condicion, reocrro lista de hijos del arbol
			foreach (var elem in arbol.getHijos())
			{
				//Llamado recursivo para realizas inspeccion en elementos hijo
				listaBot(elem, ataque);
			}
		}
		private List<Planeta> estrategiaBot(ArbolGeneral<Planeta> arbol)
		{
			List<Planeta> ataque = new List<Planeta>();
			List<Planeta> caminoAbot = new List<Planeta>();
			//Enviamos el arbol y la lista para agregar planetas a atacar
			listaBot(arbol, ataque);
			//Guardo el planeta con mayor poblacion
			Planeta maximo = ataque[ataque.Count - 1];
			//Camino hacia la poblacion con mayor numeracion
			EstrategiaBot(arbol, caminoAbot, maximo);
			//Retorno del camino hacia el Bot
			return caminoAbot;
		}
		//Estrategia para atacar al planeta con mayor poblacion
		private bool EstrategiaBot(ArbolGeneral<Planeta> arbol, List<Planeta> caminoAbot, Planeta maximo)
		{
			bool camino = false;
			//Agrego la raiz del arbol al camino
			caminoAbot.Add(arbol.getDatoRaiz());
			//Si el planeta en la lista de ataque es raiz, camino es verdadero
			if (arbol.getDatoRaiz() == maximo)
			{
				camino = true;
			}
			else
			{
				//Sino, recorro la lista de hijos, buscando el camino
				foreach (var elem in arbol.getHijos())
				{
					//Llamado recursivo para saber si hay camino
					camino = EstrategiaBot(elem, caminoAbot, maximo);
					//Si hay camino, cortamos y retornamos el camino
					if (camino)
					{
						break;
					}
					//Sino, quitamos el ult elemento de la lista
					caminoAbot.RemoveAt(caminoAbot.Count - 1);
				}
			}
			return camino;
		}
		private List<Planeta> calculoEstrategia(List<Planeta> ataque, List<Planeta> caminoAjugador)
		{
			List<Planeta> Ancestro = new List<Planeta>();
			List<Planeta> botAjugador = new List<Planeta>();
			Planeta ancestro1;
			bool existe = false;
			//Bucle hasta que el ataque o el camino alcancen el limite
			for (int i = 0; i < ataque.Count && i < caminoAjugador.Count; i++)
			{
				//Si el elemento actual se encuentra en lista de ataque
				if (ataque.Contains(caminoAjugador[i]))
				{
					//Lo agrego a la lista de ancestro
					Ancestro.Add(caminoAjugador[i]);
				}
			}
			//Asignamos el ultimo elemento de la lista
			ancestro1 = Ancestro[Ancestro.Count - 1];
			//Bucle for hasta que el primer elemento de la lista de planetas ataque
			for (int i = ataque.Count - 1; i >= 0; i--)
			{
				//Add a la lista de planetas del recorrido hacia el jugador con el elemento actual
				botAjugador.Add(ataque[i]);
				//Si el elemento actual de la lista, es igual al elemento de ancestro1
				//estamos en el mismo planeta y corta
				if (ataque[i] == ancestro1)
				{
					break;
				}
			}
			foreach (var elem in caminoAjugador)
			{
				if (existe)
				{
					//Agrego en lista bot a jugador, el elemento para continuar el camino
					botAjugador.Add(elem);
				}
				//Si elemento es igual a ancestro1, existe y terminamos el recorrido
				if (elem == ancestro1)
				{
					existe = true;
				}
			}
			//Retorno del camino
			return botAjugador;
		}
	}
}
