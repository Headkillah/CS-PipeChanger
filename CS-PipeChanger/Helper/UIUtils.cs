using System.Collections.Generic;
using UnityEngine;
using ColossalFramework.UI;

namespace SamsamTS
{
    public static class UIUtils
    {
        // Figuring all this was a pain (no documentation whatsoever)
        // So if your are using it for your mod consider thanking me (SamsamTS)
        // Extended Public Transport UI's code helped me a lot so thanks a lot AcidFire

        private static UIFont _font;

        /// <summary>
        /// Creates and position a drag handle to allow the panel to be moved by its title bar.
        /// </summary>
        public static UIDragHandle CreateDragHandle(UIComponent parent, float width, float height)
        {
            UIDragHandle dragHandle = (UIDragHandle)parent.AddUIComponent<UIDragHandle>();
            //dragHandle.width = width;
            //dragHandle.height = height;
            //dragHandle.relativePosition = Vector3.zero;
            dragHandle.area = new Vector4(0, 0, width, height);
            dragHandle.target = parent;
            return dragHandle;
        }

        public static UIButton CreateButton(UIComponent parent, string name, string text, Vector2 position)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.name = name;
            button.atlas = GetAtlas("Ingame");
            button.position = position;
            button.text = text;
            button.textPadding = new RectOffset(0, 0, 4, 0);
            button.textScale = 0.9f;
            button.normalBgSprite = "ButtonMenu";
            button.hoveredBgSprite = "ButtonMenuHovered";
            button.pressedBgSprite = "ButtonMenuPressed";
            button.disabledBgSprite = "ButtonMenuDisabled";
            button.disabledTextColor = new Color32(80, 80, 80, 128);
            button.canFocus = false;
            //button.playAudioEvents = true;

            return button;
        }

        public static UIButton CreateCloseButton(UIComponent parent)
        {
            UIButton button = parent.AddUIComponent<UIButton>();
            button.width = 32f;
            button.height = 32f;
            button.normalBgSprite = "buttonclose";
            button.hoveredBgSprite = "buttonclosehover";
            button.pressedBgSprite = "buttonclosepressed";
            button.relativePosition = new Vector3(parent.width - button.width -35, 5f);

            return button;
        }

        public static UILabel CreateTitleLabel(UIComponent parent, string name, string text, Vector2 position)
        {
            UILabel label = parent.AddUIComponent<UILabel>();
            label.name = name;
            label.text = text;
            label.textAlignment = UIHorizontalAlignment.Center;
            label.textScale = 1.2f;
            label.height = 45f;
            label.opacity = 0.8f;
            label.font = _font;
            label.relativePosition = position;
            label.padding = new RectOffset(10, 10, 5, 15);
            return label;
        }

        public static UILabel CreateLabel(UIComponent parent, string name, string text, Vector2 position)
        {
            UILabel label = parent.AddUIComponent<UILabel>();                      
            label.name = name;
            label.text = text;
            label.position = position;
            label.textScale = 1.1f;
            label.font = _font;

            return label;
        }

        public static UICheckBox CreateCheckBox(UIComponent parent)
        {
            UICheckBox checkBox = parent.AddUIComponent<UICheckBox>();

            checkBox.width = 300f;
            checkBox.height = 20f;
            checkBox.clipChildren = true;

            UISprite sprite = checkBox.AddUIComponent<UISprite>();
            sprite.atlas = GetAtlas("Ingame");
            sprite.spriteName = "ToggleBase";
            sprite.size = new Vector2(16f, 16f);
            sprite.relativePosition = Vector3.zero;

            checkBox.checkedBoxObject = sprite.AddUIComponent<UISprite>();
            ((UISprite)checkBox.checkedBoxObject).atlas = GetAtlas("Ingame");
            ((UISprite)checkBox.checkedBoxObject).spriteName = "ToggleBaseFocused";
            checkBox.checkedBoxObject.size = new Vector2(16f, 16f);
            checkBox.checkedBoxObject.relativePosition = Vector3.zero;

            checkBox.label = checkBox.AddUIComponent<UILabel>();
            checkBox.label.text = " ";
            checkBox.label.textScale = 0.9f;
            checkBox.label.relativePosition = new Vector3(22f, 2f);

            return checkBox;
        }

        public static UITextField CreateTextField(UIComponent parent, int width, int heigth, Vector3 position)
        {
            UITextField textField = parent.AddUIComponent<UITextField>();

            textField.atlas = GetAtlas("Ingame");
            textField.size = new Vector2(width, heigth);
            textField.position = position;
            textField.padding = new RectOffset(6, 6, 3, 3);
            textField.builtinKeyNavigation = true;
            textField.isInteractive = true;
            textField.readOnly = false;
            textField.horizontalAlignment = UIHorizontalAlignment.Center;
            textField.selectionSprite = "EmptySprite";
            textField.selectionBackgroundColor = new Color32(0, 172, 234, 255);
            textField.normalBgSprite = "TextFieldPanelHovered";
            textField.disabledBgSprite = "TextFieldPanelHovered";
            textField.textColor = new Color32(0, 0, 0, 255);
            textField.disabledTextColor = new Color32(80, 80, 80, 128);
            textField.color = new Color32(255, 255, 255, 255);

            return textField;
        }

        public static UIDropDown CreateDropDown(UIComponent parent, int width, int height, Vector3 position)
        {
            UIDropDown dropDown = parent.AddUIComponent<UIDropDown>();

            dropDown.atlas = GetAtlas("Ingame");
            dropDown.size = new Vector2(width, height);
            dropDown.position = position;
            dropDown.listBackground = "GenericPanelLight";
            dropDown.itemHeight = 30;
            dropDown.itemHover = "ListItemHover";
            dropDown.itemHighlight = "ListItemHighlight";
            dropDown.normalBgSprite = "TextFieldPanel";
            dropDown.focusedBgSprite = "TextFieldPanelHovered";
            dropDown.hoveredBgSprite = "TextFieldPanelHovered";
            dropDown.listPosition = UIDropDown.PopupListPosition.Above;
            dropDown.listWidth = 90;
            dropDown.listHeight = 500;
            dropDown.foregroundSpriteMode = UIForegroundSpriteMode.Stretch;
            dropDown.popupColor = new Color32(45, 52, 61, 255);
            dropDown.popupTextColor = new Color32(170, 170, 170, 255);
            dropDown.zOrder = 1;
            dropDown.textColor = new Color32(0, 0, 0, 255);
            dropDown.textScale = 0.8f;
            dropDown.verticalAlignment = UIVerticalAlignment.Middle;
            dropDown.horizontalAlignment = UIHorizontalAlignment.Left;
            dropDown.selectedIndex = 0;
            dropDown.textFieldPadding = new RectOffset(8, 0, 8, 0);
            dropDown.itemPadding = new RectOffset(14, 0, 8, 0);

            UIButton button = dropDown.AddUIComponent<UIButton>();
            dropDown.triggerButton = button;
            button.atlas = GetAtlas("Ingame");
            button.text = "";
            button.size = dropDown.size;
            button.relativePosition = new Vector3(0f, 0f);
            button.textVerticalAlignment = UIVerticalAlignment.Middle;
            button.textHorizontalAlignment = UIHorizontalAlignment.Left;
            button.normalFgSprite = "OptionsScrollbarThumb";
            button.spritePadding = new RectOffset(0, 3, 3, 0);
            button.foregroundSpriteMode = UIForegroundSpriteMode.Fill;
            button.horizontalAlignment = UIHorizontalAlignment.Right;
            button.verticalAlignment = UIVerticalAlignment.Middle;
            button.zOrder = 0;
            button.textScale = 0.8f;

            dropDown.eventSizeChanged += new PropertyChangedEventHandler<Vector2>((c, t) =>
            {
                button.size = t; dropDown.listWidth = (int)t.x;
            });

            return dropDown;
        }

        public static UICheckBox CreateIconToggle(UIComponent parent, string atlas, string checkedSprite, string uncheckedSprite)
        {
            UICheckBox checkBox = parent.AddUIComponent<UICheckBox>();

            checkBox.width = 35f;
            checkBox.height = 35f;
            checkBox.clipChildren = true;

            UIPanel panel = checkBox.AddUIComponent<UIPanel>();
            panel.atlas = GetAtlas("Ingame");
            panel.backgroundSprite = "IconPolicyBaseRect";
            panel.size = checkBox.size;
            panel.relativePosition = Vector3.zero;

            checkBox.eventCheckChanged += (c, b) =>
            {
                if (checkBox.isChecked)
                    panel.backgroundSprite = "IconPolicyBaseRect";
                else
                    panel.backgroundSprite = "IconPolicyBaseRectDisabled";
                panel.Invalidate();
            };

            checkBox.eventMouseEnter += (c, p) =>
            {
                panel.backgroundSprite = "IconPolicyBaseRectHovered";
            };

            checkBox.eventMouseLeave += (c, p) =>
            {
                if (checkBox.isChecked)
                    panel.backgroundSprite = "IconPolicyBaseRect";
                else
                    panel.backgroundSprite = "IconPolicyBaseRectDisabled";
            };

            UISprite sprite = panel.AddUIComponent<UISprite>();
            sprite.atlas = GetAtlas(atlas);
            sprite.spriteName = uncheckedSprite;
            sprite.size = checkBox.size;
            sprite.relativePosition = Vector3.zero;

            checkBox.checkedBoxObject = sprite.AddUIComponent<UISprite>();
            ((UISprite)checkBox.checkedBoxObject).atlas = sprite.atlas;
            ((UISprite)checkBox.checkedBoxObject).spriteName = checkedSprite;
            checkBox.checkedBoxObject.size = checkBox.size;
            checkBox.checkedBoxObject.relativePosition = Vector3.zero;

            return checkBox;
        }

        private static UIColorField _colorFIeldTemplate;

        public static UIColorField CreateColorField(UIComponent parent)
        {
            // Creating a ColorField from scratch is tricky. Cloning an existing one instead.

            if (_colorFIeldTemplate == null)
            {
                // Get the LineTemplate (PublicTransportDetailPanel)
                UIComponent template = UITemplateManager.Get("LineTemplate");
                if (template == null) return null;

                // Extract the ColorField
                _colorFIeldTemplate = template.Find<UIColorField>("LineColor");
                if (_colorFIeldTemplate == null) return null;
            }

            UIColorField colorField = UnityEngine.Object.Instantiate<GameObject>(_colorFIeldTemplate.gameObject).GetComponent<UIColorField>();
            parent.AttachUIComponent(colorField.gameObject);

            colorField.size = new Vector2(40f, 26f);
            colorField.pickerPosition = UIColorField.ColorPickerPosition.LeftAbove;

            return colorField;
        }

        public static void ResizeIcon(UISprite icon, Vector2 maxSize)
        {
            icon.width = icon.spriteInfo.width;
            icon.height = icon.spriteInfo.height;

            if (icon.height == 0) return;

            float ratio = icon.width / icon.height;

            if (icon.width > maxSize.x)
            {
                icon.width = maxSize.x;
                icon.height = maxSize.x / ratio;
            }

            if (icon.height > maxSize.y)
            {
                icon.height = maxSize.y;
                icon.width = maxSize.y * ratio;
            }
        }

        private static Dictionary<string, UITextureAtlas> _atlases;

        public static UITextureAtlas GetAtlas(string name)
        {
            if (_atlases == null || !_atlases.ContainsKey(name))
            {
                _atlases = new Dictionary<string, UITextureAtlas>();

                UITextureAtlas[] atlases = Resources.FindObjectsOfTypeAll(typeof(UITextureAtlas)) as UITextureAtlas[];
                for (int i = 0; i < atlases.Length; i++)
                {
                    if (!_atlases.ContainsKey(atlases[i].name))
                        _atlases.Add(atlases[i].name, atlases[i]);
                }
            }

            return _atlases[name];
        }

        public static UIFont Font
        {
            get
            {
                if (_font == null)
                {
                    _font = GameObject.Find("(Library) PublicTransportInfoViewPanel").GetComponent<PublicTransportInfoViewPanel>().Find<UILabel>("Label").font;
                }
                return _font;
            }
        }
    }
}
