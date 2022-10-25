using UnityEngine;

namespace Data
{
public interface ITextureProvider
{
    Texture2D GetTexture(string id);
}
}