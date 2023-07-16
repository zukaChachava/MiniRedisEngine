use std::collections::HashMap;

pub struct Engine{
    data: HashMap<String, String>,
}

impl Engine{
    pub fn new() -> Engine{
        Engine {data: HashMap::new()}
    }

    pub fn process_message(message: [u8; 256]){
        let splitted_parts = message.split(|item| *item == 0);
    }

    fn add(&mut self, key: String, value: String) -> (){
        self.data.insert(key, value);
    }

    fn remove(&mut self, key: &String) -> (){
        self.data.remove(key);
    }
}