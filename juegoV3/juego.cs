using System;

// ===================== EQUIPO =====================
class Equipo
{
    private int modificadorAtaque;
    private int modificadorArmadura;

    public Equipo(int modificadorAtaque, int modificadorArmadura)
    {
        this.modificadorAtaque = modificadorAtaque;
        this.modificadorArmadura = modificadorArmadura;
    }

    public int GetModificadorAtaque() => modificadorAtaque;
    public int GetModificadorArmadura() => modificadorArmadura;
}

// ===================== ARMA =====================
class Arma : Equipo
{
    public Arma(int modificadorAtaque) : base(modificadorAtaque, 0) { }
}

// ===================== ARMADURA =====================
class Armadura : Equipo
{
    public Armadura(int modificadorArmadura) : base(0, modificadorArmadura) { }
}

// ===================== PERSONAJE =====================
class Personaje
{
    private string nombre;
    private int vida;
    private int ataque;
    private Equipo equipo;

    public Personaje(string nombre, int vida, int ataque)
    {
        this.nombre = nombre;
        this.vida = vida;
        this.ataque = ataque;
        this.equipo = null;
    }

    public string GetNombre() => nombre;
    public int GetVida() => vida;

    public virtual int GetAtaque()
    {
        int bono = (equipo != null) ? equipo.GetModificadorAtaque() : 0;
        return ataque + bono;
    }

    public virtual int GetArmadura()
    {
        return (equipo != null) ? equipo.GetModificadorArmadura() : 0;
    }

    // 🔥 CLAVE PARA SIMULTANEIDAD
    public virtual int CalcularDanio()
    {
        return GetAtaque();
    }

    public virtual void AplicarDanio(int danio)
    {
        RecibirDanio(danio);
    }

    public virtual void RecibirDanio(int danio)
    {
        int danioTotal = danio - GetArmadura();
        if (danioTotal < 1) danioTotal = 1;

        vida -= danioTotal;
        if (vida < 0) vida = 0;

        if (vida == 0)
            Console.WriteLine($"{nombre} recibe {danioTotal} puntos de daño. Ha muerto :(");
        else
            Console.WriteLine($"{nombre} recibe {danioTotal} puntos de daño.");
    }

    public void Equipar(Equipo equipo)
    {
        this.equipo = equipo;
    }

    public void QuitarEquipo()
    {
        this.equipo = null;
    }

    public bool EstaVivo() => vida > 0;
}

// ===================== SACERDOTE =====================
class Sacerdote : Personaje
{
    private static Random random = new Random();

    public Sacerdote(string nombre, int vida, int ataque)
        : base(nombre, vida, ataque) { }

    public override void RecibirDanio(int danio)
    {
        if (random.Next(1, 5) == 1)
        {
            Console.WriteLine($"Las plegarias de {GetNombre()} han sido escuchadas");
            danio = (int)Math.Round(danio / 2.0);
        }

        base.RecibirDanio(danio);
    }
}

// ===================== BARBARO =====================
class Barbaro : Personaje
{
    private int furia;

    public Barbaro(string nombre, int vida, int ataque, int furia)
        : base(nombre, vida, ataque)
    {
        this.furia = furia;
    }

    public override int CalcularDanio()
    {
        int danio = GetAtaque();

        if (furia >= 3)
        {
            Console.WriteLine($"{GetNombre()} ataca furioso");
            danio = (int)Math.Round(danio * 1.15);
            furia -= 3;
        }
        else
        {
            Console.WriteLine($"{GetNombre()} está cansado");
            danio = (int)Math.Round(danio * 0.5);
        }

        return danio;
    }
}

// ===================== MUSASHI =====================
class Musashi : Personaje
{
    private static Random random = new Random();
    private bool desarmar = false;

    public Musashi(string nombre, int vida, int ataque)
        : base(nombre, vida, ataque) { }

    public override int CalcularDanio()
    {
        desarmar = false;

        if (random.Next(1, 5) == 1)
        {
            Console.WriteLine($"{GetNombre()} intentará desarmar!");
            desarmar = true;
        }

        return GetAtaque();
    }

    public override void AplicarDanio(int danio)
    {
        base.AplicarDanio(danio);
    }

    public void AplicarHabilidad(Personaje objetivo)
    {
        if (desarmar)
        {
            Console.WriteLine($"{GetNombre()} desarma a {objetivo.GetNombre()}!");
            objetivo.QuitarEquipo();
        }
    }

    public override int GetArmadura()
    {
        return 0;
    }
}

// ===================== JUEGO =====================
class Juego
{
    static void Main()
    {
       ronda(); 
    }

    public static void ronda();
    {
        Personaje p1 = new Barbaro("Dave", 30, 8, 10);
        Personaje p2 = new Sacerdote("Samson", 30, 7);

        p1.Equipar(new Arma(3));
        p2.Equipar(new Armadura(2));

        Personaje ganador = Batalla(p1, p2);

        if (ganador != null)
        {
            Console.WriteLine($"\n{ganador.GetNombre()} ha ganado la batalla");

            Personaje musashi = new Musashi("Musashi", 10, 4);
            musashi.Equipar(new Arma(2));


            Console.WriteLine("\n--- Segunda batalla ---\n");

            Personaje ganadorFinal = Batalla(musashi, ganador);

            if (ganadorFinal != null)
                Console.WriteLine($"\n{ganadorFinal.GetNombre()} ha ganado la batalla final");
            else
                Console.WriteLine("\nNo hubo ganador en la batalla final");
        //SI ES NULO es empate y han muerto? 
        } 
        else
        {
           Console.WriteLine("ambos personajes han muerto porlo que Musashi no tiene con quien pelear"); 
        }
        
    }

    public static Personaje Batalla(Personaje p1, Personaje p2)
    {
        Console.WriteLine("\n--- COMIENZA LA BATALLA ---\n");

        while (p1.EstaVivo() && p2.EstaVivo())
        {
            Console.WriteLine($"{p1.GetNombre()} Lucha con {p2.GetNombre()}");
            // Console.WriteLine($"{p2.GetNombre()} ataca a {p1.GetNombre()}");

            int danioP1 = p1.CalcularDanio();
            int danioP2 = p2.CalcularDanio();

            p2.AplicarDanio(danioP1);
            p1.AplicarDanio(danioP2);
            Console.WriteLine($"-{p1.GetNombre()} VIDA RESTANTE: " + p1.GetVida());
            Console.WriteLine($"-{p2.GetNombre()} VIDA RESTANTE: " + p2.GetVida());

            // Habilidad especial Musashi después del daño
            if (p1 is Musashi m1) m1.AplicarHabilidad(p2);
            if (p2 is Musashi m2) m2.AplicarHabilidad(p1);

            Console.WriteLine();
        }

        if (p1.EstaVivo()) return p1;
        if (p2.EstaVivo()) return p2;

        return null;
    }
}
