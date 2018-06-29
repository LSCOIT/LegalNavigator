export interface Resources {
  url: string; 
  itemId: string; 
}

export interface PlanSteps {
  topicId: string;
  topicName: string;
  steps: Array<Steps>[];
}

export interface Steps {
  id: string;
  title: string;
  type: string;
  description: string;
  order: string;
  isCompleted: boolean;
}


