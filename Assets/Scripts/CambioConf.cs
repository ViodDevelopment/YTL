using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CambioConf : MonoBehaviour
{
    SingletonLenguage.Lenguage last;
    int lastNum;
    public List<Text> textos = new List<Text>();

    private void Start()
    {
        last = SingletonLenguage.GetInstance().GetLenguage();
        lastNum = GameManager.palabrasUserDisponibles.Count;
        textos[35].text = GameManager.palabrasUserDisponibles.Count + " de 8";
        if (last == SingletonLenguage.Lenguage.CATALAN)
            DoCatalan();
        else if (last == SingletonLenguage.Lenguage.CASTELLANO)
            DoCastellano();
    }

    private void Update()
    {
        if(SingletonLenguage.GetInstance().GetLenguage() != last)
        {
            last = SingletonLenguage.GetInstance().GetLenguage();
            if (last == SingletonLenguage.Lenguage.CATALAN)
                DoCatalan();
            else if (last == SingletonLenguage.Lenguage.CASTELLANO)
                DoCastellano();
        }
        if (lastNum != GameManager.palabrasUserDisponibles.Count)
        {
            textos[35].text = GameManager.palabrasUserDisponibles.Count + " de 8";
            lastNum = GameManager.palabrasUserDisponibles.Count;
        }
    }

    private void DoCatalan()
    {
        textos[0].text = "CONFIGURACIÓ";
        textos[1].text = "AFEGIR PARAULA";
        textos[2].text = "Tipus de lletra:";
        textos[3].text = "Repeticions del exercici:";
        textos[4].text = "La paraula apareix amb o sense article?:";
        textos[5].text = "Ajuda Visual:";
        textos[6].text = "Reforç positiu:";
        textos[7].text = "Paquet de vocabulari:";
        textos[8].text = "Sense article";
        textos[9].text = "Art. determinat";
        textos[10].text = "Activat";
        textos[11].text = "Desactivat";
        textos[12].text = "Activat";
        textos[13].text = "Desactivat";
        textos[14].text = "Tots";
        textos[15].text = "Bàsics";
        textos[16].text = "Animals";
        textos[17].text = "YO TAMBIÉN LEO és una apliació educativa.\nEt recomanem llegir els consells d'ús que trobaràs en:";
        textos[18].text = "Imatge";
        textos[19].text = "Imatge";
        textos[20].text = "Paraules Creades";
        textos[21].text = "Text";
        textos[22].text = "Paraula";
        textos[23].text = "Síl·labes";
        textos[24].text = "Àudio";
        textos[25].text = "Mantingues el botó apretat per gravar";
        textos[26].text = "Escolta com ha quedat la teva gravació";
        textos[27].text = "ELIMINAR PARAULA";
        textos[28].text = "GUARDAR PARAULA";
        textos[29].text = "Accedeix a configuració";
        textos[30].text = "Creació i diseny:";
        textos[31].text = "Il·lustracions:";
        textos[32].text = "Programació:";
        textos[33].text = "Amb el suport i assessorament pedagògic de:";
        textos[34].text = "Amb la col·laboració de:";

        textos[36].text = "Art. indeterminat";
        textos[37].text = "Aquesta és la versió ESPECIAL de 'Yo también leo'\n\nVols instal·lar la versió completa?";
        textos[38].text = "ARA NO";
        textos[39].text = "INSTAL·LAR";
        textos[40].text = "Disponible en la próxima actualización.";

    }

    private void DoCastellano()
    {
        textos[0].text = "CONFIGURACIÓN";
        textos[1].text = "AÑADIR PALABRA";
        textos[2].text = "Tipo de letra:";
        textos[3].text = "Repeticiones del ejercicio:";
        textos[4].text = "¿La palabra aparece con o sin articulo?:";
        textos[5].text = "Ayuda Visual:";
        textos[6].text = "Refuerzo positivo:";
        textos[7].text = "Paquete de vocabulario:";
        textos[8].text = "Sin artículo";
        textos[9].text = "Art. determinado";
        textos[10].text = "Activado";
        textos[11].text = "Desactivado";
        textos[12].text = "Activado";
        textos[13].text = "Desactivado";
        textos[14].text = "Todos";
        textos[15].text = "Básicos";
        textos[16].text = "Animales";
        textos[17].text = "YO TAMBIÉN LEO es una apliación educativa.\n Te recomendamos leer los consejos de uso que encontrarás en:";
        textos[18].text = "Imagen";
        textos[19].text = "Imagen";
        textos[20].text = "Palabras Creadas";
        textos[21].text = "Texto";
        textos[22].text = "Palabra";
        textos[23].text = "Sílabas";
        textos[24].text = "Audio";
        textos[25].text = "Mantén el botón apretado para grabar";
        textos[26].text = "Escucha como quedó tu grabación";
        textos[27].text = "ELIMINAR PALABRA";
        textos[28].text = "GUARDAR PALABRA";
        textos[29].text = "Accede a configuración";
        textos[30].text = "Creación y diseño:";
        textos[31].text = "Ilustraciones:";
        textos[32].text = "Programación:";
        textos[33].text = "Con el apoyo y asesoramiento pedagógico de:";
        textos[34].text = "Con la colaboración de:";

        textos[36].text = "Art. indeterminado";
        textos[37].text = "Esta es la versión ESPECIAL de 'Yo también leo'\n\n¿Quieres instalar la versión completa?";
        textos[38].text = "CANCELAR";
        textos[39].text = "INSTALAR";
        textos[40].text = "Disponible en la próxima actualización.";

    }

}
