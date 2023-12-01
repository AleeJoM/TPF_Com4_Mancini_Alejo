using System;
using System.Collections.Generic;

namespace DeepSpace
{
	public class ArbolGeneral<T>
	{
		
		private T dato;
		private List<ArbolGeneral<T>> hijos = new List<ArbolGeneral<T>>();

		public ArbolGeneral(T dato) {
			this.dato = dato;
		}
		
		public T getDatoRaiz() {
			return this.dato;
		}
		
		public List<ArbolGeneral<T>> getHijos() {
			return hijos;
		}
		
		public void agregarHijo(ArbolGeneral<T> hijo) {
			this.getHijos().Add(hijo);
		}
		
		public void eliminarHijo(ArbolGeneral<T> hijo) {
			this.getHijos().Remove(hijo);
		}
		
		public bool esHoja() {
			return this.getHijos().Count == 0;
		}
		
		public int altura() {
			return 0;
		}
		
		public int nivel(ArbolGeneral<T> dato)
		{
			Cola<ArbolGeneral<T>> cola = new Cola<ArbolGeneral<T>>();			
			
			int longitud = 0;
			bool corte = false;
			
			cola.encolar(this);
			cola.encolar(null);
			
			//Mientras la cola se encuentre vacia y el corte sea falso
			while(!cola.esVacia() && corte == false)
			{
				//Desencolo el arbol para ir guardandolo
				ArbolGeneral<T> ab1 = cola.desencolar();
				if(ab1 == null)
				{
					if(!cola.esVacia())
					{
						//Incremetno la longitud a medida que voy desencolando y encolando nodos
						//mientras este llena
						longitud++;
						cola.encolar(null);
					}
				}
				else
				{
					//Si el arbol es igual al dato, condicion de corte verdadera
					if(ab1.Equals(dato))
					{
						corte = true;
					}
					//Encolamos los elementos de nuevo en la cola
					foreach(var elem in ab1.hijos)
					{
						cola.encolar(elem);
					}
				}
			}
			//Retornamos la longitud, que es la cantidad total del camino
			return longitud;
		}
	}
}