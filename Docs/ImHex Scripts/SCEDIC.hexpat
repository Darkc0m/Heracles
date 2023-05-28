struct string {
    char s[];
};

struct pointer32 {
    string* pointer32 : u32;
};

struct scedic {
    u32 entries;
    pointer32 ptr[entries + 1];
};

scedic file @ 0x00;
