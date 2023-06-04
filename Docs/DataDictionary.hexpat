struct string {
    char s[];
};

struct pointer32 {
    string* pointer32 : u32;
};

struct dataDictionary {
    u32 numEntries;
    pointer32 ptr[numEntries * 11 + 1];
};

dataDictionary file @ 0x00;
