public enum PANELID
{
    ID_LOBBY = 0,
    ID_BATTLE,
    ID_MAX,
};

public enum STATPANELID
{
    ID_NONE = -1,
    ID_INVENTORY,
    ID_DETAILSTAT,
    ID_SKILLSTAT,
};

public enum BLACKSMITISUBMENUID
{
    ID_BUY = 0,
    ID_UPGRADE,
    ID_SOCKET
};

public enum MONSTERGRADEID
{
    GRADE_ERROR = -1,
    GRADE_NORMAL = 0,
    GRADE_ELITE,
    GRADE_BOSS,
    GRADE_LEGEND,
    GRADE_GOD,
    GRADE_HIDDEN,
    GRADE_MAX,
};

public enum FIELDID
{
    ID_ERROR = -1,
    ID_VILAGE = 0,
    ID_CASTLE,
    ID_BATTLEFIELD01,
    ID_BATTLEFIELD02,
    ID_DONGEON,
    ID_SUDDENATTACK,
    ID_MAX,
};

public enum Moveable_Type
{
    TYPE_PLAYER = 0,
    TYPE_MONSTER,
    TYPE_RESOURCE,
    TYPE_NONE,
    TYPE_MAX,
};

public enum ATKTYPEID
{
    ATT_MELEE = 0,
    ATT_RANGE,
    ATT_MAGIC,
    ATT_SUMMON,
    ATT_MAX,
};

public enum SUMMONPOSITIONID
{
    POSITION_INFIELD = 0,
    POSITION_OUTFIELD,
    POSITION_INANDOUT,
    POSITION_UPPERINFIELD,
    POSITION_MAX,
};

//==============Item==================//
public enum ITEMTYPEID
{
    ITEM_EQUIP,
    ITEM_POTION,
    ITEM_FOOD,
    ITEM_GEM,
};


public enum ITEMGRADEID
{
    ITEMGRADE_NORMAL = 0,
    ITEMGRADE_MAGIC,
    ITEMGRADE_RARE,
    ITEMGRADE_UNIQUE,
    ITEMGRADE_LEGEND,
    ITEMGRADE_MAX,
}

public enum EQUIPMENTTYPEID
{
    EQUIP_WEAPON,
    EQUIP_ARMOR,
    EQUIP_GLOVE,
    EQUIP_FOOTS,
    EQUIP_TROUSERS,
    EQUIP_MAX,
};

public enum SUPPLIESEFFECTID
{
    EFFECT_HP,
    EFFECT_STAMINA,
    EFFECT_MANA,
    EFFECT_MAX,
};

//==============Stat && JobSystem==================//
public enum PLAYERSTATKINDSID
{
    STAT_BASIC = 0,
    STAT_CLASS,
    STAT_CRAFTMAN,
    STAT_MAX,
};

public enum PLAYERBASICSTATID
{
    STAT_BASIC_STR,
    STAT_BASIC_DEX,
    STAT_BASIC_INT,
    STAT_BASIC_MAX,
};

public enum JOBKINDSID
{
    JOB_KNIGHT = 0,
    JOB_PRIEST,
    JOB_MAGICIAN,
    JOB_MAX,
};

//==============Misc==================//
public enum BULLETTYPEID
{
    BULLET_STRAIGHT,
    BULLET_STRAIGHT_TOANGLE,
    BULLET_CURVE_TARGET,
}

public enum PLAYEVENTID
{
    EVENT_NULL = -1,
    EVENT_CANIVALTIME,
    EVENT_GOLDRUSH,
    EVENT_MAX,
}

public enum COLLIDERDIRID
{
    COL_UPPER,
    COL_LOWER,
    COL_LEFT,
    COL_RIGHT,
    COL_MAX,
}