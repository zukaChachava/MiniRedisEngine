pub enum Methods{
    Get = 1,
    Add = 2,
    Remove = 3,
    Update = 4
}

pub struct Response{
    pub response_type: ResponseType,
    pub data: String
}

pub enum ResponseType{
    Error = 1,
    Data = 2,
    NotFound = 3
}

pub const SEPERATOR : u8 = 0x0;
pub const DEFAULT_PORT: i32 = 9009;
