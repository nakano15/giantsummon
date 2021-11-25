using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace giantsummon
{
    public class GuardianSprites
    {
        public Texture2D HeadSprite
        {
            get
            {
                if (!IsTextureLoaded || _HeadSprite.IsDisposed)
                    LoadTextures();
                return _HeadSprite;
            }
            set
            {
                _HeadSprite = value;
            }
        }
        public Texture2D BodySprite
        {
            get
            {
                if (!IsTextureLoaded || _BodySprite.IsDisposed)
                    LoadTextures();
                return _BodySprite;
            }
            set
            {
                _BodySprite = value;
            }
        }
        public Texture2D LeftArmSprite
        {
            get
            {
                if (!IsTextureLoaded || _LeftArmSprite.IsDisposed)
                    LoadTextures();
                return _LeftArmSprite;
            }
            set
            {
                _LeftArmSprite = value;
            }
        }
        public Texture2D RightArmSprite
        {
            get
            {
                if (!IsTextureLoaded || _RightArmSprite.IsDisposed)
                    LoadTextures();
                return _RightArmSprite;
            }
            set
            {
                _RightArmSprite = value;
            }
        }
        private Texture2D _HeadSprite, _BodySprite, _LeftArmSprite, _RightArmSprite;
        public Texture2D BodyFrontSprite, RightArmFrontSprite;
        private bool TexturesLoaded = false, ErrorLoading = false;
        public bool IsTextureLoaded { get { return TexturesLoaded; } }
        public bool HasErrorLoadingOccurred { get { return ErrorLoading; } }
        private string SpriteDir { get { return "Creatures/" + ReferedBase.Name; } }
        private Mod mod = null;
        private Dictionary<string, ExtraTextureHolder> ExtraTextures = new Dictionary<string, ExtraTextureHolder>();
        public GuardianBase ReferedBase;

        public GuardianSprites(GuardianBase companionBase, Mod mod = null)
        {
            ReferedBase = companionBase;
            if (mod == null)
                mod = MainMod.mod;
            this.mod = mod;
            //this.SpriteDir = "Creatures/" + ReferedBase.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TextureKey">Name used to locate the texture when getting it</param>
        /// <param name="TextureFileName">the file name of the texture only. The directory of loading is the character folder.</param>
        public void AddExtraTexture(string TextureKey, string TextureFileName)
        {
            if (!ExtraTextures.ContainsKey(TextureKey))
            {
                ExtraTextureHolder eth = new ExtraTextureHolder(TextureFileName);
                ExtraTextures.Add(TextureKey, eth);
            }

        }

        /// <summary>
        /// Check if texture isn't null before drawing.
        /// </summary>
        public Texture2D GetExtraTexture(string TextureKey)
        {
            if (ExtraTextures.ContainsKey(TextureKey))
            {
                if (!ExtraTextures[TextureKey].TextureLoaded)
                    ExtraTextures[TextureKey].LoadTexture(SpriteDir, mod);
                return ExtraTextures[TextureKey].Texture;
            }
            return null;
        }

        public void LoadTextures()
        {
            if (ErrorLoading)
                return;
            try
            {
                if (ReferedBase.IsCustomSpriteCharacter)
                {
                    HeadSprite = mod.GetTexture(SpriteDir + "/head");
                    BodySprite = mod.GetTexture(SpriteDir + "/body");
                    LeftArmSprite = mod.GetTexture(SpriteDir + "/left_arm");
                    RightArmSprite = mod.GetTexture(SpriteDir + "/right_arm");
                    if (mod.TextureExists(SpriteDir + "/body_front"))
                        BodyFrontSprite = mod.GetTexture(SpriteDir + "/body_front");
                    if (mod.TextureExists(SpriteDir + "/right_arm_front"))
                        RightArmFrontSprite = mod.GetTexture(SpriteDir + "/right_arm_front");
                }
                foreach (ExtraTextureHolder eth in ExtraTextures.Values)
                {
                    eth.LoadTexture(SpriteDir, mod);
                }
                TexturesLoaded = true;
            }
            catch
            {
                TexturesLoaded = false;
                ErrorLoading = true;
                Terraria.Main.NewText("Error loading " + ReferedBase.Name+ "'s texture.");
            }
        }

        protected class ExtraTextureHolder
        {
            public Texture2D Texture;
            public string TextureFileName;
            public bool TextureLoaded { get { return Texture != null; } }

            public ExtraTextureHolder(string TextureFileName)
            {
                this.TextureFileName = TextureFileName;
                Texture = null;
            }

            public void LoadTexture(string SpriteDir, Mod mod)
            {
                if(mod.TextureExists(SpriteDir + "/" + TextureFileName))
                {
                    Texture = mod.GetTexture(SpriteDir + "/" + TextureFileName);
                }
                else
                {
                    Texture = null;
                }
            }
        }
    }
}
