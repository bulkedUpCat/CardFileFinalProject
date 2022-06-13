export class EmailParams implements EmailParameters{
  title: boolean;
  category: boolean;
  author: boolean;
  datePublished: boolean;
}

export interface EmailParameters{
  title: boolean;
  category: boolean;
  author: boolean;
  datePublished: boolean;
}
