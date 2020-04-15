using UnityEngine;

public static class SiLoTienesBienSinoPaCasa
{
    public static Sprite GetSpriteFromUser(Sprite _sprite, bool puzzle = false)
    {
        try
        {
            Sprite l_sprite = _sprite;
            Rect newrect;
            /*  if((l_sprite.texture.width / l_sprite.texture.height) > 0.9f && (l_sprite.texture.width / l_sprite.texture.height) < 1.1f || Mathf.Abs(l_sprite.texture.width - l_sprite.texture.height) < 30)
              {
                  return l_sprite;
              }
              else if (l_sprite.texture.width > l_sprite.texture.height)
                  newrect = new Rect(new Vector2(l_sprite.texture.width / 4, 0), new Vector2(l_sprite.texture.width / (l_sprite.texture.width / l_sprite.texture.height), l_sprite.texture.width / (l_sprite.texture.width / l_sprite.texture.height)));
              else
                  newrect = new Rect(new Vector2(0, l_sprite.texture.height / 4), new Vector2(l_sprite.texture.height / (l_sprite.texture.height / l_sprite.texture.width), l_sprite.texture.height / (l_sprite.texture.height / l_sprite.texture.width)));
                  */
            if (l_sprite.texture.width > l_sprite.texture.height)
            {
                if ((float)l_sprite.texture.width / (float)l_sprite.texture.height < 2f)
                {
                    if (!puzzle)
                        newrect = new Rect(new Vector2((float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height) / 2f, (float)l_sprite.texture.height / ((float)l_sprite.texture.width / (float)l_sprite.texture.height) / 2), new Vector2((float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height), (float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height)));
                    else
                        newrect = new Rect(new Vector2(l_sprite.texture.width / 8, 0), new Vector2((float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height), (float)l_sprite.texture.width / ((float)l_sprite.texture.width / (float)l_sprite.texture.height)));

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
                        newrect = new Rect(new Vector2(0,l_sprite.texture.height / 8), new Vector2((float)l_sprite.texture.height / ((float)l_sprite.texture.height / (float)l_sprite.texture.width), (float)l_sprite.texture.height / ((float)l_sprite.texture.height / (float)l_sprite.texture.width)));

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

    public static float GetSizePuzzle(Sprite l_sprite, out bool ancho/* ,out Vector2 _pos*/)
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
}
