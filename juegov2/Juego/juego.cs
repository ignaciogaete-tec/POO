using System;

//--------------------------- CLASE EQUIPO
public abstract class Equipo
{
    // Propiedades automáticas con getter público
    public int ModificadorAtaque { get; }
    public int ModificadorArmadura { get; }

    // Constructor
    public Equipo(int modAtaque, int modArmadura)
    {
        ModificadorAtaque = modAtaque;
        ModificadorArmadura = modArmadura;
    }
}

//--------------------------- SUBCLASE ARMA
public class Arma : Equipo
{
    public Arma(int modAtaque) : base(modAtaque, 0) { }
}

//--------------------------- SUBCLASE ARMADURA
public class Armadura : Equipo
{
    public Armadura(int modArmadura) : base(0, modArmadura) { }
}

//--------------------------- CLASE PERSONAJE
public class Personaje
{
    // Atributos
    private string nombre;
    private int vida;
    private int ataque;
    private Equipo? equipo;

    // Constructor
    public Personaje(string nombre, int vida, int ataque)
    {
        this.nombre = nombre;
        this.vida = vida;
        this.ataque = ataque;
        this.equipo = null;
    }

    // Getters
    public string GetNombre() => nombre;
    public int GetVida() => vida;

    public virtual int GetAtaque()
    {
        int bono = (equipo != null) ? equipo.ModificadorAtaque : 0;
        return ataque + bono;
    }

    public int GetArmadura()
    {
        return (equipo != null) ? equipo.ModificadorArmadura : 0;
    }

    // Métodos
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

//--------------------------- CLASE JUEGO
class Juego
{
    public static void Main()
    {
        Console.WriteLine("Iniciando Juego...");
        
        // Crear personajes
        Personaje guerrero = new Personaje("Guerrero", 100, 20);
        Personaje mago = new Personaje("Mago", 80, 25);
        
        // Crear equipamiento
        Arma espada = new Arma(15);
        Armadura armaduraMetal = new Armadura(10);
        
        // Equipar personajes
        guerrero.Equipar(espada);
        mago.Equipar(armaduraMetal);
        
        // Simular batalla
        Console.WriteLine($"\n--- BATALLA ---");
        Console.WriteLine($"{guerrero.GetNombre()} - Vida: {guerrero.GetVida()}, Ataque total: {guerrero.GetAtaque()}");
        Console.WriteLine($"{mago.GetNombre()} - Vida: {mago.GetVida()}, Ataque total: {mago.GetAtaque()}, Armadura: {mago.GetArmadura()}");
        
        Console.WriteLine($"\n--- COMIENZA EL COMBATE ---");
        guerrero.Atacar(mago);
        mago.Atacar(guerrero);
        
        Console.WriteLine($"\n--- ESTADO FINAL ---");
        Console.WriteLine($"{guerrero.GetNombre()} - Vida: {guerrero.GetVida()}");
        Console.WriteLine($"{mago.GetNombre()} - Vida: {mago.GetVida()}");
    }
}