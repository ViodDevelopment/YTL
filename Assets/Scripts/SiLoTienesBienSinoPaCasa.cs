/*using UnityEngine;

public static class SiLoTienesBienSinoPaCasa
{
    public static Sprite GetSpriteFromUser(Sprite _sprite, bool puzzle = false)
    {
        try
        {
            Sprite l_sprite = _sprite;
            Rect newrect;
  
            if (l_sprite.texture.width > l_sprite.texture.height)
            {
                if ((float)l_sprite.texture.width / (float)l_sprite.texture.height < 2f)
                {
                    if (!puzzle)
                        newrect = new Rect(new Vector2((float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height) / 2f, (float)l_sprite.texture.height / ((float)l_sprite.texture.width / (float)l_sprite.texture.height) / 2), new Vector2((float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height), (float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height)));
                    else
                    {
                        float distancia = (float)l_sprite.texture.height;
                        float separacion = (l_sprite.texture.width - distancia) / 2f;
                        newrect = new Rect(new Vector2(separacion, 0), new Vector2(distancia,distancia));
                    }
                }
                else
                    newrect = new Rect(new Vector2((float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height) / 2f, 0), new Vector2((float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height), (float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height)));

            }
            else
            {
                if ((float)l_sprite.texture.height / (float)l_sprite.texture.width < 2)
                {
                    if (!puzzle)
                        newrect = new Rect(new Vector2((float)l_sprite.texture.width / ((float)l_sprite.texture.height / (float)l_sprite.texture.width) / 2f, (float)l_sprite.texture.height / ((float)l_sprite.texture.height / (float)l_sprite.texture.width) / 2), new Vector2((float)l_sprite.texture.height / ((float)l_sprite.texture.height / (float)l_sprite.texture.width), (float)l_sprite.texture.height / ((float)l_sprite.texture.height / (float)l_sprite.texture.width)));
                    else
                    {
                        float distancia = (float)l_sprite.texture.height;
                        float separacion = (l_sprite.texture.width - distancia) / 2f;
                        newrect = new Rect(new Vector2(0,separacion), new Vector2(distancia, distancia));
                    }
                }
                else
                    newrect = new Rect(new Vector2(0, (float)l_sprite.texture.height / ((float)l_sprite.texture.height / (float)l_sprite.texture.width) / 2f), new Vector2((float)l_sprite.texture.height / ((float)l_sprite.texture.height / (float)l_sprite.texture.width), (float)l_sprite.texture.height / ((float)l_sprite.texture.height / (float)l_sprite.texture.width)));
            }
            l_sprite = Sprite.Create(l_sprite.texture, newrect, Vector2.zero);
            return l_sprite;
        }
        catch
        {
            return _sprite;
        }
    }

    public static float GetSizePuzzle(Sprite l_sprite, out bool ancho)
    {

        if (l_sprite.texture.width > l_sprite.texture.height)
        {
            ancho = true;
            return (float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height) / 2f;

            // _pos = new Vector2(_sprite.texture.width / (_sprite.texture.width / _sprite.texture.height * 2), _sprite.texture.height / (_sprite.texture.width / _sprite.texture.height * 2));
            // return _sprite.texture.width / (_sprite.texture.width / _sprite.texture.height) / 2;

        }
        else
        {

            ancho = false;
            //_pos = new Vector2(_sprite.texture.width / (_sprite.texture.height / _sprite.texture.width * 2), _sprite.texture.height / (_sprite.texture.height / _sprite.texture.width * 2));
            //return _sprite.texture.height / (_sprite.texture.height / _sprite.texture.width) / 2;
            return (float)l_sprite.texture.height / ((float)l_sprite.texture.height / (float)l_sprite.texture.width) / 2f;
        }
    }
}*/

using UnityEngine;

public static class SiLoTienesBienSinoPaCasa
{
    public static Sprite GetSpriteFromUser(Sprite _sprite, bool puzzle = false)
    {
        try
        {
            Sprite l_sprite = _sprite;
            Rect newrect;
            if (l_sprite.texture.width > l_sprite.texture.height)
                newrect = new Rect(new Vector2(l_sprite.texture.width / 4, 0), new Vector2(l_sprite.texture.width / 2, l_sprite.texture.width / 2));
            else
                newrect = new Rect(new Vector2(0, l_sprite.texture.height / 4), new Vector2(l_sprite.texture.height / 2, l_sprite.texture.height / 2));

            l_sprite = Sprite.Create(l_sprite.texture, newrect, Vector2.zero);
            return l_sprite;
        }
        catch
        {
            return _sprite;
        }
    }

    public static float GetSizePuzzle(Sprite _sprite, out bool ancho)
    {
        if (_sprite.texture.width > _sprite.texture.height)
        {
            ancho = true;
            return _sprite.texture.width / 4;
        }
        else
        {
            ancho = false;
            return _sprite.texture.height / 4;
        }
    }
}
