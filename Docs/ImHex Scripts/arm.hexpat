#define numBattle 58
#define numStatus 47
#define numLocations 275
#define outsLoc 19
#define numAbilities 47
#define numSpells 80
#define numEnemySkills 113
#define numPlayerSkills 173


struct string {
    char s[];
};

struct pointer16 {
    string* relativeTextPointer16 : u16 [[pointer_base("pointer_offset")]];
};

struct location{
    char string[];
    $ = addressof(string) + 0x24;
};

struct ability{
    $ += 0x10;
    char name[];
    $ = addressof(name) + 0x14;
    string* description : u32 [[pointer_base("arm_address")]];
};

struct spell {
    char name[];
    $ = addressof(name) + 0x40;
    string* description : u32 [[pointer_base("arm_address")]];
    $ = addressof(name) + 0x44;
};

struct enemySkill {
    char name[];
    $ = addressof(name) + 0x34;
    string* description : u32 [[pointer_base("arm_address")]];
    $ = addressof(name) + 0x38;
};

struct playerSkill {
    $ += 0x14;
    char name[];
    $ = addressof(name) + 0x14;
    if(name != "\x00")
        string* description : u32 [[pointer_base("arm_address")]];
    $ = addressof(name) + 0x3C;
};

struct battleDialog {
    u32 pointer [[hidden]];
    if(pointer != 0){
        $ -= sizeof(pointer);
        string* dialog : u32 [[pointer_base("arm_address")]];
    }
};

struct status {
    padding[6];
    char name[];
    $ = addressof(name) + 0x18;
};

struct outskirtsLocation {
    char name[];
    $ = addressof(name) + 0x24;
};

struct combatData {
    ability abilities[numAbilities];
    spell spells[numSpells];
    enemySkill enemySkills[numEnemySkills];
    playerSkill playerSkills[numPlayerSkills];
    padding[0x0C];
    battleDialog battleDialogs[931];
};

struct battle {
    padding[4];
    u32 pointer [[hidden]];
    if(pointer != 0){
        $ -= sizeof(pointer);
        string* dialog : u32 [[pointer_base("arm_address")]];
    }
};

fn arm_address (u32 address){
    return -0x2000000;
};

battle battle[numBattle] @ 0x0ED054;
status status[numStatus] @ 0x0ED25C;
outskirtsLocation outskirtsLocations[outsLoc] @ 0x0F12D0;
location locations[numLocations] @ 0x0F76BC;
combatData combatData @ 0x113644;

