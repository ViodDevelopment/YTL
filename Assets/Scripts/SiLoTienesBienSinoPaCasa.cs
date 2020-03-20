using UnityEngine;

public static class SiLoTienesBienSinoPaCasa
{
    public static Sprite GetSpriteFromUser(Sprite _sprite)
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
