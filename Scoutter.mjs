import { Agent, run, tool } from "@openai/agents";
import { z } from "zod";

const searchFigma = tool({
    name: "search_figma",
    description: "Return Figma search links for a query",
    parameters: z.object({
        query: z.string(),
    }),
    execute: async ({ query }) => {
        const url = `https://www.google.com/search?q=${encodeURIComponent(
            query + " figma community"
        )}`;

        return `Search this: ${url}`;
    },
});

const agent = new Agent({
    name: "Figma Scout",
    // Use backticks (`) for multi-line strings
    instructions: `Find and analyze UI designs on Figma. 
    
    Must-haves for your use case:
    - Clean sidebar navigation (Modules: Students, Teachers, Groups, Lessons, Subjects, Departments, Faculties)
    - Dashboard with stats cards (student counts, active lessons, attendance rates)
    - Data tables with pagination (list views)
    - Form layouts (create/edit forms)

    Look for these on Figma community:
    - Search "LMS dashboard UI" or "Education admin panel"
    - Search "University management system UI"
    - Adminty (customization is an option)

    Things to avoid:
    - Overly complex designs with features you won't build
    - Mobile-first designs
    - Designs without form/table components`,

    tools: [searchFigma],
});

const result = await run(agent, "Find LMS dashboard UI designs on Figma Community");
console.log(result.finalOutput);