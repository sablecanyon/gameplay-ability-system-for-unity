namespace GAS.General
{
    public static class GASTextDefine
    {
        public const string TITLE_SETTING = "Settings";
        public const string TITLE_PATHS = "Path";
        public const string TITLE_BASE_INFO = "Base Info";
        public const string TITLE_DESCRIPTION = "Description";

        
        #region GASSettingAsset

        public const string TIP_CREATE_GEN_AscUtilCode =
            "<color=white><size=15>Before generating the ASC extension class, you must ensure that the collection tool class of Ability and AttributeSet has been generated. Because the ASC extension class depends on this</size></color>";

        public const string TIP_CREATE_FOLDERS =
            "<color=white><size=15>If you have modified the EX-GAS configuration asset path, please click this button to ensure that all subfolders and base configuration files are generated correctly.</size></color>";

        public const string LABEL_OF_CodeGeneratePath = "Script generation path";
        public const string LABEL_OF_GASConfigAssetPath = "Configuration file Asset path";
        public const string BUTTON_CheckAllPathFolderExist = " Check subdirectories and basic configuration";
        public const string BUTTON_GenerateAscExtensionCode = " Generate AbilitySystemComponentExtension class script";

        #endregion

        
        #region Tag

        public const string BUTTON_ExpandAllTag = "Expand All";
        public const string BUTTON_CollapseAllTag = "Collapse All";
        public const string BUTTON_AddTag = "AddTag";
        public const string BUTTON_RemoveTag = "RemoveTag";
        public const string BUTTON_GenTagCode = "Generate TagLib";

        #endregion
        
        
        #region Attribute
        public const string TIP_Warning_EmptyAttribute =
            "<size=13><color=yellow><color=orange>Attribute name</color>Not allowed<color=red><b>null</b></color>! " +
            "Please check!</color></size>";
        public const string BUTTON_GenerateAttributeCollection = " Generate AttrLib";
        
        public const string TIP_Warning_DuplicatedAttribute =
            "<size=13><color=yellow>The <color=orange>Attribute name</color> prohibited <color=red><b>repetition</b></color>!\n" +
            "Duplicate Attributes name:<size=15><b><color=white> {0} </color></b></size>.</color></size>";
        #endregion


        #region AttributeSet

        public const string ERROR_DuplicatedAttribute = "<size=16><b>There are duplicate attributes!</b></size>";
        public const string ERROR_Empty = "<size=16><b>AttributeSet must have at least one Attribute!</b></size>";
        public const string ERROR_EmptyName = "<size=16><b>AttributeSet name cannot be empty!</b></size>";
        public const string ERROR_InElements = "<size=16><b><color=orange>Please fix the AttributeSet prompt error first!</color></b></size>";
        
        public const string ERROR_DuplicatedAttributeSet = "<size=16><b><color=orange>Duplicate AttributeSet exists!\n" +
                                                           "<color=white> ->  {0}</color></color></b></size>";
        public const string BUTTON_GenerateAttributeSetCode = " Generate AttrSetLib";
        
        #endregion


        #region GameplayEffect
        
        public const string TIP_BASE_INFO="For descriptive purposes only, to facilitate understanding.";
        public const string TIP_GE_POLICY="Instant = instantaneous, Duration = continuous, Infinite = permanent";
        public const string LABLE_GE_NAME = "Effect Name";
        public const string LABLE_GE_DESCRIPTION = "Effect description";
        public const string TITLE_GE_POLICY="Gameplay Effect Implementation Strategy";
        public const string LABLE_GE_POLICY = "Time limit strategy";
        public const string LABLE_GE_DURATION = "duration";
        public const string LABLE_GE_INTERVAL = "Intervals";
        public const string LABLE_GE_EXEC = "Interval Effect";
        public const string TITLE_GE_GrantedAbilities = "Grant Ability";
        public const string TITLE_GE_MOD = "Modifier";
        public const string TITLE_GE_TAG = "Tag";
        public const string TITLE_GE_CUE = "Cue";
      
        public const string TITLE_GE_TAG_AssetTags = "AssetTags - The tags of the [game effect] itself";
        public const string TIP_GE_TAG_AssetTags = "AssetTags: Tags are used to describe the specific attributes of [game effects] themselves, including but not limited to damage, treatment, control and other effect types.\nThese tags help differentiate and define the role and performance of [game effects].\nCan be used with RemoveGameplayEffectsWithTags.";
        public const string TITLE_GE_TAG_GrantedTags = "GrantedTags - Tags granted to the target unit";
        public const string TIP_GE_TAG_GrantedTags = "GrantedTags: Tags are added to the target unit when [Game Effect] is enabled, and removed when [Game Effect] is disabled.\nThis tag is invalid for Instant [game effects].";
        public const string TITLE_GE_TAG_ApplicationRequiredTags = "ApplicationRequiredTags - All prerequisites for applying this [game effect]";
        public const string TIP_GE_TAG_ApplicationRequiredTags = "ApplicationRequiredTags: The target unit of [Game Effect] must have [all] of these tags in order to be applied to the target unit.\nIf you want to express that [any] tag cannot be applied to the target, you should use the ApplicationImmunityTags tag.";
        public const string TITLE_GE_TAG_OngoingRequiredTags = "OngoingRequiredTags - All the prerequisites for activating this game effect";
        public const string TIP_GE_TAG_OngoingRequiredTags = "OngoingRequiredTags: The target unit of the [Game Effect] must have [all] of these tags, otherwise the effect will not be triggered.\nOnce a [Game Effect] is applied, if the target unit's tags change during the effect, resulting in it no longer having [all] of these tags, the effect will be invalid; otherwise, if the conditions are met, the effect will be activated.\nThis tag is invalid for Instant [Game Effects].";
        public const string TITLE_GE_TAG_RemoveGameplayEffectsWithTags = "RemoveGameplayEffectsWithTags - Removes [GameplayEffects] with [any] tags";
        public const string TIP_GE_TAG_RemoveGameplayEffectsWithTags = "RemoveGameplayEffectsWithTags: All Gameplay Effects currently held by the target unit will have any of these tags removed from their AssetTags or GrantedTags.";
        public const string TITLE_GE_TAG_ApplicationImmunityTags = "ApplicationImmunityTags - cannot be applied to a target with any of the tags";
        public const string TIP_GE_TAG_ApplicationImmunityTags = "ApplicationImmunityTags: This [game effect] cannot be applied to a target unit with [any] of these tags.";
        
        public const string TITLE_GE_CUE_CueOnExecute = "CueOnExecute - Triggered when executed";
        public const string TITLE_GE_CUE_CueDurational = "CueDurational - Triggers continuously while it exists";
        public const string TITLE_GE_CUE_CueOnAdd = "CueOnAdd - Triggered when adding";
        public const string TITLE_GE_CUE_CueOnRemove = "CueOnRemove - Triggered on remove";
        public const string TITLE_GE_CUE_CueOnActivate = "CueOnActivate - Triggered when activated";
        public const string TITLE_GE_CUE_CueOnDeactivate = "CueOnDeactivate - Triggered on deactivation";
        
        public const string LABEL_GRANT_ABILITY = "Grant Abilities";
        public const string LABEL_GRANT_ABILITY_LEVEL = "Ability level";
        public const string LABEL_GRANT_ABILITY_ACTIVATION_POLICY = "Activation Policy";
        public const string LABEL_GRANT_ABILITY_DEACTIVATION_POLICY = "Deactivation Policy";
        public const string LABEL_GRANT_ABILITY_REMOVE_POLICY = "Remove Policy";
        public const string TIP_GRANT_ABILITY_ACTIVATION_POLICY = "None = If it is not activated, the user needs to manually call the ASC related interface to activate it; " +
                                                                  "WhenAdded = Activate when added;" +
                                                                  "SyncWithEffect = Synchronize with GE, activate when GE is activated"; 
        public const string TIP_GRANT_ABILITY_DEACTIVATION_POLICY = "None = no relevant deactivation logic, user needs to call ASC to deactivate; " +
                                                                  "SyncWithEffect = Sync with GE, deactivate when GE is inactive";   
        public const string TIP_GRANT_ABILITY_REMOVE_POLICY = "None = do not remove capabilities;" +
                                                              "SyncWithEffect = Sync with GE, remove when GE is removed" +
                                                              "WhenEnd = When the ability ends, remove itself;" +
                                                              "WhenCancel = When the ability is canceled, remove itself;" +
                                                              "WhenCancelOrEnd = When the ability is canceled or ended, remove itself";
        
        public const string TITLE_GE_STACKING = "Stack Configuration";
        public const string LABEL_GE_STACKING_CODENAME = "Stacked GE ID";
        public const string LABEL_GE_STACKING_TYPE = "Stack Type";
        public const string LABEL_GE_STACKING_COUNT = "Stack Limit maybe count TODO";
        public const string LABEL_GE_STACKING_DURATION_REFRESH_POLICY = "Duration refresh policy";
        public const string LABEL_GE_STACKING_PERIOD_RESET_POLICY = "Period reset policy";
        public const string LABEL_GE_STACKING_EXPIRATION_POLICY = "Expiration policy";
        public const string LABEL_GE_STACKING_DENY_OVERFLOW_APPLICATION = "Overflowed GE does not take effect";
        public const string LABEL_GE_STACKING_CLEAR_STACK_ON_OVERFLOW = "Clear stack on overflow";
        public const string LABEL_GE_STACKING_CLEAR_OVERFLOW_EFFECTS = "GE triggered on overflow";
        
        
        #endregion

        #region Ability

        public const string ABILITY_BASEINFO="基本信息";
        public const string TIP_UNAME =
            "<size=12><b><color=white><color=orange>U-Name is very important!</color>" +
            "GAS uses U-Name as the identifier of the Ability." +
            "So you must ensure the uniqueness of the U-Name." +
            "Don't worry, the tool will remind you of this when generating the AbilityLib.</color></b></size>";
        public const string ABILITY_CD_TIME="Cooldown duration";
        public const string ABILITY_EFFECT_CD="Cooling effect";
        public const string ABILITY_EFFECT_COST="Consumption effect";
        public const string ABILITY_MANUAL_ENDABILITY = "Manual end ability";
        public const string BUTTON_CHECK_TIMELINE_ABILITY = "View/Edit Ability Timeline";

        #endregion
        
        #region ASC
        
        public const string TIP_ASC_BASEINFO="The basic information is only used to describe this ASC to facilitate designers to read and understand the ASC.";
        public const string ASC_BASE_TAG="Intrinsic Tags";
        public const string ASC_BASE_ABILITY="Intrinsic Abilities";
        public const string ASC_AttributeSet="AttributeSet";
        
        #endregion

        #region Watcher

        public const string TIP_WATCHER = "This window is used to monitor the running status of GAS. It is recommended to open this window when debugging the role abilities and effects of GAS.";
        public const string TIP_WATCHER_OnlyForGameRunning = 
            "<size=20><b><color=yellow>The monitor is only available while the game is running.</color></b></size>";

        #endregion

        #region Gameplay Cue

        public const string CUE_ANIMATION_PATH = "Animator relative path";
        public const string CUE_ANIMATION_INCLUDE_CHILDREN = "Include child nodes";
        public const string CUE_ANIMATION_INCLUDE_CHILDREN_ANIMATOR_TIP = "Search for animators in the node itself and its children. This option can be checked when the path of your animator is not completely certain (for example, different skin nodes are inconsistent).";
        public const string CUE_ANIMATION_STATE = "Animation State Name";
        public const string CUE_ANIMATION_PATH_TIP = "Empty means the root node of the object, for example: 'A' = child object named 'A' under the root node, 'A/B' = child object named 'B' under the 'A' node";

        public const string CUE_SOUND_EFFECT = "Sound Effect";
        public const string CUE_ATTACH_TO_OWNER = "Whether to attach to Owner";
        
        public const string CUE_VFX_PREFAB = "VFX Prefab";
        public const string CUE_VFX_OFFSET = "VFX Offset";
        public const string CUE_VFX_ROTATION = "VFX Rotation";
        public const string CUE_VFX_SCALE = "VFX Scale";
        public const string CUE_VFX_ACTIVE_WHEN_ADDED = "Whether it is activated when added";

        #endregion
    }
}