class KafkaAdmin {
    private admin: any;

    constructor(adminClient: any) {
        this.admin = adminClient;
    }

    async createTopic(topicName: string, numPartitions: number, replicationFactor: number) {
        await this.admin.createTopics({
            topics: [{
                topic: topicName,
                numPartitions: numPartitions,
                replicationFactor: replicationFactor,
            }],
        });
    }

    async deleteTopic(topicName: string) {
        await this.admin.deleteTopics([topicName]);
    }

    async listTopics() {
        return await this.admin.listTopics();
    }

    async describeTopic(topicName: string) {
        return await this.admin.describeTopics([topicName]);
    }

    async close() {
        await this.admin.disconnect();
    }
}

export default KafkaAdmin;