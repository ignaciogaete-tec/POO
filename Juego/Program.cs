// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;

//---------------------------   CLASE EQUIPO — al mismo nivel que Juego
public abstract class Equipo
//abstract => hereda y no se puede instanciar directamente, solo a través de sus subclases (Arma y Armadura)
{
    //ATRIBUTOS
    private int modificadorAtaque;
    private int modificadorArmadura;
    //CONSTRUCTOR
    public Equipo(int modAtaque, int modArmadura)
    {
        this.modificadorAtaque = modAtaque;
        this.modificadorArmadura = modArmadura;
    }
    //METODOS
    public int GetModificadorAtaque() => modificadorAtaque;
    public int GetModificadorArmadura() => modificadorArmadura;
}

//---------------------------   SUBCLASE ARMA hereda de EQUIPO — al mismo nivel que Juego
public class Arma : Equipo
{
    public Arma(int modAtaque) : base(modAtaque, 0) { }
    //HEREDA DE EQUIPO, SOLO MODIFICADOR DE ATAQUE, ARMADURA 0
    // base(mod, 0) le dice a C#:
    // "Antes de crear la Arma, ejecuta el constructor de Equipo con estos valores".
}

//---------------------------   SUBCLASE ARMADURA hereda de EQUIPO — al mismo nivel que Juego
public class Armadura : Equipo
{
    public Armadura(int modArmadura) : base(0, modArmadura) { }
}

//---------------------------   CLASE PERSONAJE — al mismo nivel que Juego
public class Personaje
{
    //----------------------- ATRIBUTOS Personaje
    private string nombre;
    private int vida;
    private int ataque;
    private Equipo? equipo;

    //----------------------- CONTRUCTOR Personaje
    public Personaje(string nombre, int vida, int ataque)
    {
        this.nombre = nombre;
        this.vida = vida;
        this.ataque = ataque;
        this.equipo = null;
    }
    //----------------------- GETERS
    public string GetNombre() => nombre;

    public int GetVida() => vida;

    public virtual int GetAtaque()
    {
        int bono = (equipo != null) ? equipo.GetModificadorAtaque() : 0;
        return ataque + bono;
    }

    public int GetArmadura()
    {
        return (equipo != null) ? equipo.GetModificadorArmadura() : 0;
    }
    //-------------------------METODOS ADICIONALES
    public virtual void Atacar(Personaje objetivo)
    {
        Console.WriteLine($"{this.nombre} ataca a {objetivo.GetNombre()}");
        objetivo.RecibirDanio(this.GetAtaque());
    }

    public virtual void RecibirDanio(int danio)
    {
        int danioFinal = Math.Max(1, danio - GetArmadura());
        this.vida = Math.Max(0, this.vida - danioFinal);

        Console.WriteLine($"{this.nombre} recibe {danioFinal} puntos de daño");

        if (this.vida <= 0)
        {
            Console.WriteLine($"{this.nombre} ha muerto :(");
        }
    }

    public void Equipar(Equipo nuevoEquipo)
    {
        this.equipo = nuevoEquipo;
    }
}

//---------------------------   CLASE JUEGO — punto de entrada del programa
class Juego
{
    // Aquí es donde inicia TODO
    public static void Main()           // El método Main es donde inicia TODO
    {
        Console.WriteLine("Iniciando Juego...");
        // Aquí  métodos más adelante
        // Batalla(p1, p2);
    }
}

