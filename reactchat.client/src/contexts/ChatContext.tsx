export interface Message {
    sender: string;
    content: string;
    type: 'sender' | 'recipient';
}
