using System;
using ColossalFramework.UI;
using ThemeMixer.Locale;
using ThemeMixer.Resources;
using ThemeMixer.Themes.Enums;
using ThemeMixer.TranslationFramework;
using ThemeMixer.UI.Abstraction;
using ThemeMixer.UI.Parts.OffsetPanels;
using ThemeMixer.UI.Parts.TexturePanels;
using UnityEngine;

namespace ThemeMixer.UI.CategoryPanels
{
    public class TerrainPanel : PanelBase
    {

        private UIPanel _labelPanel;
        private UILabel _label;
        private UIButton _loadButton;

        private PanelBase _container;
        private PanelBase _panelLeft;
        private PanelBase _panelCenter;
        private PanelBase _panelRight;

        public GrassPollutionPanel GrassPollutionColorOffset;
        public GrassFieldPanel GrassFieldColorOffset;
        public GrassFertilityPanel GrassFertilityColorOffset;
        public GrassForestPanel GrassForestColorOffset;

        public PanelBase DetailPanel;
        public PanelBase DetailPanelInner;

        public CheckboxPanel CliffDetail;
        public CheckboxPanel FertileDetail;
        public CheckboxPanel GrassDetail;

        public override void Awake()
        {
            base.Awake();
            Category = ThemeCategory.Terrain;
            Setup("Terrain Panel", 1070.0f, 0.0f, 0, true, LayoutDirection.Vertical, LayoutStart.TopLeft, "GenericPanel");
            this.CreateSpace(1.0f, 5.0f);
            CreateLabel();
            CreateContainers();
            CreateLeftSideTexturePanels();
            CreateMiddleTexturePanels();
            CreateRightSideTexturePanels();
            CreateLeftSideOffsetPanels();
            CreateMiddleSideOffsetPanels();
            CreateDetailPanel();
            this.CreateSpace(1.0f, 5.0f);
            DetailPanel.color = UIColorGrey;
        }

        private void CreateLabel()
        {
            _labelPanel = AddUIComponent<UIPanel>();
            _labelPanel.size = new Vector2(width, 22.0f);
            _label = _labelPanel.AddUIComponent<UILabel>();
            _label.font = UIUtils.BoldFont;
            _label.textScale = 1.0f;
            _label.textAlignment = UIHorizontalAlignment.Center;
            _label.verticalAlignment = UIVerticalAlignment.Middle;
            _label.padding = new RectOffset(0, 0, 4, 0);
            _label.text = Translation.Instance.GetTranslation(TranslationID.LABEL_TERRAIN);
            _label.anchor = UIAnchorStyle.CenterHorizontal | UIAnchorStyle.CenterVertical;
            string loadTooltip = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_LOADFROMTHEME);
            _loadButton = UIUtils.CreateButton(_label, new Vector2(22.0f, 22.0f), tooltip: loadTooltip, backgroundSprite: "ThemesIcon", atlas: UISprites.Atlas);
            _loadButton.relativePosition = new Vector2(_label.width + 5.0f, 0.0f);
            _loadButton.eventClicked += OnLoadTerrainFromTheme;
        }

        private void OnLoadTerrainFromTheme(UIComponent component, UIMouseEventParameter eventParam)
        {
            Controller.OnLoadFromTheme(ThemeCategory.Terrain, ThemeCategory.Terrain);
        }

        private void CreateContainers()
        {
            _container = AddUIComponent<PanelBase>();
            _container.Setup("Terrain Container", 0.0f, 460.0f, 5, true);
            _container.autoFitChildrenVertically = true;
            _panelLeft = _container.AddUIComponent<PanelBase>();
            _panelLeft.Setup("Terrain Panel Left", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
            _panelCenter = _container.AddUIComponent<PanelBase>();
            _panelCenter.Setup("Terrain Panel Center", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
            _panelRight = _container.AddUIComponent<PanelBase>();
            _panelRight.Setup("Terrain Panel Right", 350.0f, 0.0f, 0, true, LayoutDirection.Vertical);
        }

        private void CreateLeftSideTexturePanels()
        {
            _panelLeft.AddUIComponent<GrassDiffusePanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<RuinedDiffusePanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            _panelLeft.AddUIComponent<GravelDiffusePanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
        }

        private void CreateMiddleTexturePanels()
        {
            _panelCenter.AddUIComponent<CliffDiffusePanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
            _panelCenter.AddUIComponent<SandDiffusePanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
            _panelCenter.AddUIComponent<CliffSandNormalPanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
        }

        private void CreateRightSideTexturePanels()
        {
            _panelRight.AddUIComponent<PavementDiffusePanel>();
            _panelRight.CreateSpace(1.0f, 5.0f);
            _panelRight.AddUIComponent<OilDiffusePanel>();
            _panelRight.CreateSpace(1.0f, 5.0f);
            _panelRight.AddUIComponent<OreDiffusePanel>();
            _panelRight.CreateSpace(1.0f, 5.0f);
        }

        private void CreateLeftSideOffsetPanels()
        {
            GrassFertilityColorOffset = _panelLeft.AddUIComponent<GrassFertilityPanel>();
            _panelLeft.CreateSpace(1.0f, 5.0f);
            GrassForestColorOffset = _panelLeft.AddUIComponent<GrassForestPanel>();
        }

        private void CreateMiddleSideOffsetPanels()
        {
            GrassPollutionColorOffset = _panelCenter.AddUIComponent<GrassPollutionPanel>();
            _panelCenter.CreateSpace(1.0f, 5.0f);
            GrassFieldColorOffset = _panelCenter.AddUIComponent<GrassFieldPanel>();
        }

        private void CreateDetailPanel()
        {
            DetailPanel = _panelRight.AddUIComponent<PanelBase>();
            DetailPanel.Setup("Terrain Detail Panel", 350.0f, 231.0f, 5, bgSprite: "WhiteRect");
            DetailPanelInner = DetailPanel.AddUIComponent<PanelBase>();
            DetailPanelInner.Setup("Terrain Detail Panel Inside", 1.0f, 1.0f, 5, true, LayoutDirection.Vertical);
            DetailPanelInner.anchor = UIAnchorStyle.CenterHorizontal | UIAnchorStyle.CenterVertical;

            UILabel label = DetailPanelInner.AddUIComponent<UILabel>();
            label.font = UIUtils.Font;
            label.textScale = 1.0f;
            label.padding = new RectOffset(4, 0, 4, 0);
            label.relativePosition = Vector2.zero;
            label.text = Translation.Instance.GetTranslation(TranslationID.LABEL_TITLE_DETAIL);

            GrassDetail = DetailPanelInner.AddUIComponent<CheckboxPanel>();
            bool grassState = Controller.GetValue<bool>(ValueID.GrassDetailEnabled);
            string grassLabelText = Translation.Instance.GetTranslation(TranslationID.LABEL_VALUE_GRASSDETAIL);
            string grassTooltipText = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_VALUE_GRASSDETAIL);
            GrassDetail.Initialize(grassState, grassLabelText, grassTooltipText);
            GrassDetail.EventCheckboxStateChanged += OnDetailStateChanged;

            FertileDetail = DetailPanelInner.AddUIComponent<CheckboxPanel>();
            bool fertileState = Controller.GetValue<bool>(ValueID.FertileDetailEnabled);
            string fertileLabelText = Translation.Instance.GetTranslation(TranslationID.LABEL_VALUE_FERTILEDETAIL);
            string fertileTooltipText = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_VALUE_FERTILEDETAIL);
            FertileDetail.Initialize(fertileState, fertileLabelText, fertileTooltipText);
            FertileDetail.EventCheckboxStateChanged += OnDetailStateChanged;

            CliffDetail = DetailPanelInner.AddUIComponent<CheckboxPanel>();
            bool cliffState = Controller.GetValue<bool>(ValueID.RocksDetailEnabled);
            string cliffLabelText = Translation.Instance.GetTranslation(TranslationID.LABEL_VALUE_CLIFFDETAIL);
            string cliffTooltipText = Translation.Instance.GetTranslation(TranslationID.TOOLTIP_VALUE_CLIFFDETAIL);
            CliffDetail.Initialize(cliffState, cliffLabelText, cliffTooltipText);
            CliffDetail.EventCheckboxStateChanged += OnDetailStateChanged;

            DetailPanelInner.autoFitChildrenHorizontally = true;
        }

        private void OnDetailStateChanged(UIComponent comp, bool state)
        {
            CheckboxPanel cbp = comp as CheckboxPanel;
            ValueID id = ValueID.None;
            if (ReferenceEquals(cbp, CliffDetail)) id = ValueID.RocksDetailEnabled;
            if (ReferenceEquals(cbp, FertileDetail)) id = ValueID.FertileDetailEnabled;
            if (ReferenceEquals(cbp, GrassDetail)) id = ValueID.GrassDetailEnabled;
            Controller.OnValueChanged(id, state);
        }

        protected override void OnRefreshUI(object sender, UIDirtyEventArgs e)
        {
            base.OnRefreshUI(sender, e);
            try
            {
                CliffDetail.SetState(Controller.GetValue<bool>(ValueID.RocksDetailEnabled));
                FertileDetail.SetState(Controller.GetValue<bool>(ValueID.FertileDetailEnabled));
                GrassDetail.SetState(Controller.GetValue<bool>(ValueID.GrassDetailEnabled));
            }
            catch (Exception ex)
            {
                Debug.LogError(string.Concat("Error caught in TerrainPanel.RefreshUI: ", ex));
            }
        }
    }
}
