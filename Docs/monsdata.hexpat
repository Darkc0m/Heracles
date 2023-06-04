struct string {
    char s[];
};

struct monster {
    u16 id;
    u16 id2;
    padding[0x120];
    string name;
    $ = addressof(name);
    padding[0x18];
};

struct monsdata {
    u32 numEntries;
    padding[4];
    monster m[numEntries - 1];
};

monsdata file @ 0x00;


