using UnityEngine;

public static class SiLoTienesBienSinoPaCasa
{
    public static Sprite GetSpriteFromUser(Sprite _sprite)
    {
        try
        {
            Sprite l_sprite = _sprite;
            Rect newrect = new Rect(new Vector2(l_sprite.texture.width / 2 - Screen.height / 2, l_sprite.texture.height / 2 - Screen.height / 2), new Vector2(Screen.height, Screen.height));
            l_sprite =  Sprite.Create(l_sprite.texture, newrect, Vector2.zero);
            return l_sprite;
        }
        catch
        {
            return _sprite;
        }
    }
}
