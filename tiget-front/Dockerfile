# Use an official Node runtime as a parent image
FROM node:14-alpine

# Set the working directory in the container
WORKDIR /src/app

# Copy package.json and package-lock.json to the working directory
COPY package*.json ./

# Install Angular CLI globally (you can remove this line if you don't need Angular CLI)
RUN npm install -g @angular/cli

# Install app dependencies
RUN npm install

# Install react-scripts globally
RUN npm install -g react-scripts

# Copy the rest of the application code to the working directory
COPY . .

# Expose the port the app runs on
EXPOSE 3000

# Start the application
CMD ["npm", "start"]
