FROM node:16-alpine

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Install ts-node globally
# RUN npm install -g ts-node

# Build the TypeScript code
RUN npm run build

# Expose the port the app runs on
EXPOSE 3000

# Command to run the application
# CMD ["ts-node", "src/app.ts"]
# CMD ["node", "dist/app.js"]
CMD ["npm", "start"]